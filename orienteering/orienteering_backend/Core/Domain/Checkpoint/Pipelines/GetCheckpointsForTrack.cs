using AutoMapper;
using MediatR;
using Microsoft.EntityFrameworkCore;
using orienteering_backend.Core.Domain.Authentication.Services;
using orienteering_backend.Core.Domain.Checkpoint.Dto;
using orienteering_backend.Core.Domain.Track.Dto;
using orienteering_backend.Core.Domain.Track.Pipelines;
using orienteering_backend.Infrastructure.Data;
using System.Security.Authentication;

// Lisens MediatR: https://github.com/jbogard/MediatR/blob/master/LICENSE

namespace orienteering_backend.Core.Domain.Checkpoint.Pipelines
{

    public static class GetCheckpointsForTrack
    {
        public record Request(
            Guid trackId) : IRequest<List<CheckpointDto>>;


        public class Handler : IRequestHandler<Request, List<CheckpointDto>>
        {
            private readonly OrienteeringContext _db;
            private readonly IMapper _mapper;
            private readonly IMediator _mediator;
            private readonly IIdentityService _identityService;


            public Handler(OrienteeringContext db, IMapper mapper, IMediator mediator, IIdentityService identityService )
            {
                _db = db ?? throw new ArgumentNullException(nameof(db));
                _mapper = mapper;
                _mediator = mediator;
                _identityService = identityService;
            }
            public async Task<List<CheckpointDto>> Handle(Request request, CancellationToken cancellationToken)
            {
                //check that signed in
                var userId = _identityService.GetCurrentUserId();
                if (userId == null) { throw new AuthenticationException("user not signed in"); }

                //check that user is allowed to do this
                TrackUserIdDto track = await _mediator.Send(new GetTrackUser.Request(request.trackId));
                if(track.UserId!=userId) { throw new AuthenticationException(); }

                //get checkpoints
                var checkpointList = await _db.Checkpoints
                    .Where(c => c.TrackId == request.trackId)
                    .ToListAsync();

                //convert to dto
                var checkpointDtoList = new List<CheckpointDto>();
                for (var i = 0; i < checkpointList.Count; i++)
                {
                    var checkpoint = checkpointList[i];
                    var checkpointDto = _mapper.Map<Checkpoint, CheckpointDto>(checkpoint);
                    checkpointDtoList.Add(checkpointDto);
                }
                return checkpointDtoList;
            }
        }

    }
}
