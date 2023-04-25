using MediatR;
using orienteering_backend.Core.Domain.Checkpoint.Dto;
using orienteering_backend.Infrastructure.Data;
using orienteering_backend.Core.Domain.Checkpoint.Events;
using orienteering_backend.Core.Domain.Track.Pipelines;
using System.Security.Authentication;
using System.Net;
using orienteering_backend.Core.Domain.Authentication.Services;
using AutoMapper;

//License MediatR (Apache 2.0): https://github.com/jbogard/MediatR/blob/master/LICENSE
//License quickchart api (GNU): https://github.com/typpo/quickchart/blob/master/LICENSE
// License Automapper (MIT): https://github.com/AutoMapper/AutoMapper/blob/master/LICENSE.txt

namespace orienteering_backend.Core.Domain.Checkpoint.Pipelines;

public static class CreateCheckpoint
{
    public record Request(
        CheckpointDto checkpointDto) : IRequest<Guid>;


    public class Handler : IRequestHandler<Request, Guid>
    {
        private readonly OrienteeringContext _db;
        private readonly IMediator _mediator;
        private readonly IIdentityService _identityService;
        private readonly IMapper _mapper;
        


        public Handler(OrienteeringContext db, IMediator mediator, IIdentityService identityService, IMapper mapper)
        {
            _db = db ?? throw new ArgumentNullException(nameof(db));
            _mediator = mediator;
            _identityService = identityService;
            _mapper=mapper;
        }
        public async Task<Guid> Handle(Request request, CancellationToken cancellationToken)
        {
            //check that signed in
            var userId = _identityService.GetCurrentUserId();
            if (userId == null) { throw new AuthenticationException("user not signed in"); }
            var trackDto = await _mediator.Send(new GetSingleTrackUnauthorized.Request(request.checkpointDto.TrackId));

            //If not allowed allowed to do this then throw exception
            if (trackDto.UserId != userId) { throw new ArgumentNullException("The user dont own this track or it dosent exist."); };

            //create checkpoint
            var newCheckpoint = _mapper.Map<Checkpoint>(request.checkpointDto);
            newCheckpoint.Order = trackDto.NumCheckpoints + 1;

            if (request.checkpointDto.GameId == 0)
            {
                //no game--> cheeckpoint is quiz
                newCheckpoint.QuizId = Guid.NewGuid();
            }

            await _db.Checkpoints.AddAsync(newCheckpoint);
            await _db.SaveChangesAsync(cancellationToken);

            //qrcode
            string url = "http://152.94.160.74/checkpoint/";
            if (newCheckpoint.QuizId == null)
            {
                url += "game/" + newCheckpoint.Id.ToString();
            }
            else
            {
                url += "quiz/" + newCheckpoint.Id.ToString();
            }

            //Sources: https://www.c-sharpcorner.com/article/create-qr-code-using-google-charts-api-in-vb-net/ (31.01.2023)

            string QrLink = "https://quickchart.io/qr?text=";
            QrLink = QrLink + url;
            using (WebClient webClient = new WebClient())
            {
                const SslProtocols _Tls12 = (SslProtocols)0xC00;
                const SecurityProtocolType Tls12 = (SecurityProtocolType)_Tls12;
                ServicePointManager.SecurityProtocol = Tls12;
                byte[] data = webClient.DownloadData(QrLink);

                newCheckpoint.QRCode = data;

                await _db.SaveChangesAsync();
            }

            // publishing event 
            
            var checkpointCreatedEvent = new CheckpointCreated(newCheckpoint.Id, request.checkpointDto.TrackId);
            if (newCheckpoint.GameId == 0)
            {
                checkpointCreatedEvent.QuizId = newCheckpoint.QuizId;
            }
            await _mediator.Publish(checkpointCreatedEvent);
            return newCheckpoint.Id;
        }
    }

}
