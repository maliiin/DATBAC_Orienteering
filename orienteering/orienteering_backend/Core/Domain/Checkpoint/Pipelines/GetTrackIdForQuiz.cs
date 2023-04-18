using MediatR;
using Microsoft.EntityFrameworkCore;
using orienteering_backend.Infrastructure.Data;

// Lisens MediatR: https://github.com/jbogard/MediatR/blob/master/LICENSE

namespace orienteering_backend.Core.Domain.Checkpoint.Pipelines;

public static class GetTrackIdForQuiz
{
    public record Request(
        Guid quizId) : IRequest<Guid>;

    public class Handler : IRequestHandler<Request, Guid>
    {
        private readonly OrienteeringContext _db;
        
        public Handler(OrienteeringContext db)
        {
            _db = db ?? throw new ArgumentNullException(nameof(db));
            
        }

        //This pipeline does not verify authentication, becuase it is only called from within backend, and not from a controllerApi
        //return id of checkpoint qith spesific quizId

        public async Task<Guid> Handle(Request request, CancellationToken cancellationToken)
        {
            var checkpoint = await _db.Checkpoints
                .Where(c => c.QuizId == request.quizId)
                .FirstOrDefaultAsync(cancellationToken);

            if (checkpoint is null) { throw new ArgumentNullException("the checkpoint cannot be found"); };
            return checkpoint.TrackId;
        }
    }
}

