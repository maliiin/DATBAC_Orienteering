using MediatR;
using orienteering_backend.Infrastructure.Data;
using orienteering_backend.Core.Domain.Checkpoint;
using Microsoft.EntityFrameworkCore;

// Lisens MediatR: https://github.com/jbogard/MediatR/blob/master/LICENSE

namespace orienteering_backend.Core.Domain.Checkpoint.Pipelines;

public static class GetCheckpointDescription
{
    public record Request(
        Guid checkpointId) : IRequest<string>;


    public class Handler : IRequestHandler<Request, string>
    {
        private readonly OrienteeringContext _db;
        private readonly IMediator _mediator;


        public Handler(OrienteeringContext db, IMediator mediator)
        {
            _db = db ?? throw new ArgumentNullException(nameof(db));
            _mediator = mediator;
        }
        public async Task<string> Handle(Request request, CancellationToken cancellationToken)
        {
            var checkpoint = await _db.Checkpoints.FirstOrDefaultAsync(c => c.Id == request.checkpointId);
            if (checkpoint == null)
            {
                throw new ArgumentNullException("checkpoint not found");
            }
            var checkpointDescription = checkpoint.CheckpointDescription;
            return checkpointDescription;
        }

    }
}
