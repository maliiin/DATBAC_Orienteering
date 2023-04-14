using MediatR;
using Microsoft.EntityFrameworkCore;
using orienteering_backend.Infrastructure.Data;
using orienteering_backend.Core.Domain.Checkpoint.Dto;
using AutoMapper;
using orienteering_backend.Core.Domain.Authentication.Services;
using System.Security.Authentication;
using orienteering_backend.Core.Domain.Track.Dto;
using orienteering_backend.Core.Domain.Track.Pipelines;

namespace orienteering_backend.Core.Domain.Checkpoint.Pipelines;

public static class GetSingleCheckpoint
{
    public record Request(
        Guid checkpointId) : IRequest<CheckpointDto>;

    public class Handler : IRequestHandler<Request, CheckpointDto>
    {
        private readonly OrienteeringContext _db;
        private readonly IMapper _mapper;
        private readonly IIdentityService _identityService;
        private readonly IMediator _mediator;

        public Handler(OrienteeringContext db, IMapper mapper, IIdentityService identityService, IMediator mediator)
        {
            _db = db ?? throw new ArgumentNullException(nameof(db));
            _mapper = mapper;
            _identityService= identityService;
            _mediator = mediator;
        }

        public async Task<CheckpointDto> Handle(Request request, CancellationToken cancellationToken)
        {
            //check that signed in
            var userId = _identityService.GetCurrentUserId();
            if (userId == null) { throw new AuthenticationException("user not signed in"); }


            var checkpoint = await _db.Checkpoints
                .Where(c => c.Id == request.checkpointId)
                .FirstOrDefaultAsync(cancellationToken);

            if (checkpoint is null) { throw new NullReferenceException("the checkpoint cannot be found or not allowed to access"); };

            //check that user is allowed to access this track
            TrackUserIdDto track = await _mediator.Send(new GetTrackUser.Request(checkpoint.TrackId));
            if (userId != track.UserId) { throw new NullReferenceException("the checkpoint cannot be found or not allowed to access"); }

            //create dto
            var checkpointDto = _mapper.Map<Checkpoint, CheckpointDto>(checkpoint);
            return checkpointDto;
        }
    }

}

