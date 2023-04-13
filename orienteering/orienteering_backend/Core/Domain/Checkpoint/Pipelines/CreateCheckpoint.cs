using MediatR;
using Microsoft.EntityFrameworkCore;
using orienteering_backend.Core.Domain.Checkpoint.Dto;
using orienteering_backend.Core.Domain.Checkpoint;
using orienteering_backend.Infrastructure.Data;
using orienteering_backend.Core.Domain.Checkpoint.Events;
using orienteering_backend.Core.Domain.Track.Pipelines;
using Microsoft.AspNetCore.Http.HttpResults;
using System.Security.Authentication;
using System.Net;
//Kilder: CampusEats lab fra dat240
// Kilder: https://github.com/dat240-2022/assignments/blob/main/Lab3/UiS.Dat240.Lab3/Core/Domain/Cart/Pipelines/AddItem.cs (07.02.2023)
// Brukte samme struktur på pipelinen som i kilden

namespace orienteering_backend.Core.Domain.Checkpoint.Pipelines;

public static class CreateCheckpoint
{
    public record Request(
        CheckpointDto checkpointDto, Guid userId) : IRequest<Guid>;


    public class Handler : IRequestHandler<Request, Guid>
    {
        private readonly OrienteeringContext _db;
        private readonly IMediator _mediator;


        public Handler(OrienteeringContext db, IMediator mediator)
        {
            _db = db ?? throw new ArgumentNullException(nameof(db));
            _mediator = mediator;
        }
        public async Task<Guid> Handle(Request request, CancellationToken cancellationToken)
        {
            //get track to get numCheckpoint
            ////(fix, her kan kanskje getsingletrackunauthorizeb brukes istedenfor siden denne pipelinen allerede er sikret.)
            var trackDto = await _mediator.Send(new GetSingleTrack.Request(request.checkpointDto.TrackId));

            //Not allowed to do this
            if (trackDto.UserId != request.userId) { throw new NullReferenceException("The user dont own this track or it dosent exist."); };

            //create checkpoint
            var newCheckpoint = new Checkpoint(request.checkpointDto.Title, request.checkpointDto.GameId, request.checkpointDto.TrackId);
            newCheckpoint.Order = trackDto.NumCheckpoints + 1;

            if (request.checkpointDto.GameId == 0)
            {
                //no game--> should be quiz
                newCheckpoint.QuizId = Guid.NewGuid();
            }

            await _db.Checkpoints.AddAsync(newCheckpoint);
            await _db.SaveChangesAsync(cancellationToken);

            //qrcode
            //Kilder: https://www.c-sharpcorner.com/article/create-qr-code-using-google-charts-api-in-vb-net/ (31.01.2023)
            //Lisens quickchart api: https://github.com/typpo/quickchart (31.01.2023)
            string url = "http://152.94.160.74/checkpoint/";
            if (newCheckpoint.QuizId == null)
            {
                url += "game/" + newCheckpoint.Id.ToString();
            }
            else
            {
                url += "quiz/" + newCheckpoint.Id.ToString();
            }

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
            await _mediator.Publish(new CheckpointCreated(newCheckpoint.Id, request.checkpointDto.TrackId));

            if (newCheckpoint.GameId == 0)
            {
                //checkpoint with quiz
                await _mediator.Publish(new QuizCheckpointCreated(newCheckpoint.Id, (Guid)newCheckpoint.QuizId));
            }

            return newCheckpoint.Id;
        }
    }

}
