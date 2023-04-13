using MediatR;
using Microsoft.EntityFrameworkCore;
using orienteering_backend.Core.Domain.Checkpoint.Dto;
using orienteering_backend.Core.Domain.Checkpoint;
using orienteering_backend.Infrastructure.Data;
using orienteering_backend.Core.Domain.Checkpoint.Events;
using orienteering_backend.Core.Domain.Track.Pipelines;

namespace orienteering_backend.Core.Domain.Checkpoint.Pipelines;

public static class GetNextCheckpoint
{
    public record Request(
        Guid currentCheckpointId) : IRequest<Guid>;


    public class Handler : IRequestHandler<Request, Guid>
    {
        private readonly OrienteeringContext _db;
        private readonly IMediator _mediator;


        public Handler(OrienteeringContext db, IMediator mediator)
        {
            _db = db ?? throw new ArgumentNullException(nameof(db));
            _mediator = mediator;
        }

        //returns checkpoint id that is the next after current checkpoint
        public async Task<Guid> Handle(Request request, CancellationToken cancellationToken)
        {
            var currentCheckpoint = await _db.Checkpoints
                .Where(c => c.Id == request.currentCheckpointId)
                .FirstOrDefaultAsync(cancellationToken);
            //fix error is null
            var track = await _mediator.Send(new GetSingleTrackUnauthorized.Request(currentCheckpoint.TrackId));

            int orderNewCheckpoint=currentCheckpoint.Order+1;
            if (currentCheckpoint.Order >= track.NumCheckpoints)
            {
                //the current checkpoint was the "last"--> the next is the first
                orderNewCheckpoint = 1;
            }

            var nextCheckpoint = await _db.Checkpoints
                .Where(c => c.TrackId == currentCheckpoint.TrackId)
                .Where(c => c.Order == orderNewCheckpoint)
                .FirstOrDefaultAsync(cancellationToken);

            return nextCheckpoint.Id;


        }
    }

}
