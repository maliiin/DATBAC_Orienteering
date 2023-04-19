using MediatR;
using orienteering_backend.Infrastructure.Data;
using orienteering_backend.Core.Domain.Checkpoint.Events;

// Lisens MediatR: https://github.com/jbogard/MediatR/blob/master/LICENSE

namespace orienteering_backend.Core.Domain.Quiz.Handlers;



public class CheckpointCreatedHandler : INotificationHandler<CheckpointCreated>
{
    private readonly OrienteeringContext _db;
    public CheckpointCreatedHandler(OrienteeringContext db)
    {
        _db = db ?? throw new ArgumentNullException(nameof(db));
    }
    public async Task Handle(CheckpointCreated notification, CancellationToken cancellationToken)
    {
        var quizId = notification.QuizId;
        if (quizId != null)
        {
            var Quiz = new Quiz((Guid)quizId);
            _db.Quiz.Add(Quiz);
            await _db.SaveChangesAsync(cancellationToken);
        }
    }
}
