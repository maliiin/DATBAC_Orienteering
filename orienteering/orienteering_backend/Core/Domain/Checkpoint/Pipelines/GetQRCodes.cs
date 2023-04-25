using AutoMapper;
using MediatR;
using Microsoft.EntityFrameworkCore;
using orienteering_backend.Core.Domain.Authentication.Services;
using orienteering_backend.Core.Domain.Checkpoint.Dto;
using orienteering_backend.Core.Domain.Track.Dto;
using orienteering_backend.Core.Domain.Track.Pipelines;
using orienteering_backend.Infrastructure.Data;
using System.Security.Authentication;

// License MediatR (Apache 2.0): https://github.com/jbogard/MediatR/blob/master/LICENSE
// License Automapper (MIT): https://github.com/AutoMapper/AutoMapper/blob/master/LICENSE.txt
// License .NET EFCore (MIT) https://github.com/dotnet/efcore/blob/main/LICENSE.txt


namespace orienteering_backend.Core.Domain.Checkpoint.Pipelines;

public static class GetQRCodes
{
    public record Request(
        Guid TrackId
        ) : IRequest<List<CheckpointNameAndQRCodeDto>>;

    public class Handler : IRequestHandler<Request, List<CheckpointNameAndQRCodeDto>>
    {
        private readonly OrienteeringContext _db;
        private readonly IMapper _mapper;
        private readonly IIdentityService _identityService;
        private readonly IMediator _mediator;

        public Handler(OrienteeringContext db, IMapper mapper, IIdentityService identityService, IMediator mediator)
        {
            _db = db ?? throw new ArgumentNullException(nameof(db));
            _mapper = mapper;
            _identityService = identityService;
            _mediator = mediator;
        }

        public async Task<List<CheckpointNameAndQRCodeDto>> Handle(Request request, CancellationToken cancellationToken)
        {
            //check that signed in
            var userId = _identityService.GetCurrentUserId();
            if (userId == null) { throw new AuthenticationException("user not signed in"); }

            //check that user is allowed to access this track
            TrackUserIdDto track = await _mediator.Send(new GetTrackUser.Request(request.TrackId));
            if (userId != track.UserId) { throw new ArgumentNullException("not found or access not allowed"); }

            //get checkpoints
            var checkpointList = await _db.Checkpoints
                .Where(c => c.TrackId == request.TrackId)
                .ToListAsync();

            if (checkpointList == null)
            {
                throw new ArgumentNullException("not found or access not allowed");
            }
            var dtoList = new List<CheckpointNameAndQRCodeDto>();


            for (var i = 0; i < checkpointList.Count; i++)
            {
                var checkpoint = checkpointList[i];
                var dtoElement = _mapper.Map<Checkpoint, CheckpointNameAndQRCodeDto>(checkpoint);
                dtoList.Add(dtoElement);
            }
            return dtoList;
        }
    }

}
