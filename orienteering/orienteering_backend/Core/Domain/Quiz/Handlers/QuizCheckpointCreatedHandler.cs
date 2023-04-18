using MediatR;
using orienteering_backend.Infrastructure.Data;
using orienteering_backend.Core.Domain.Checkpoint.Events;

// Lisens MediatR: https://github.com/jbogard/MediatR/blob/master/LICENSE

namespace orienteering_backend.Core.Domain.Quiz.Handlers;



public class QuizCheckpointCreatedHandler : INotificationHandler<QuizCheckpointCreated>
{
    private readonly OrienteeringContext _db;
    public QuizCheckpointCreatedHandler(OrienteeringContext db)
    {
        _db = db ?? throw new ArgumentNullException(nameof(db));
    }
    public async Task Handle(QuizCheckpointCreated notification, CancellationToken cancellationToken)
    {

        Quiz Quiz = new(notification.QuizId);

        _db.Quiz.Add(Quiz);
        await _db.SaveChangesAsync(cancellationToken);

    }
}
