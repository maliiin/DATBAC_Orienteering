
using MediatR;
using orienteering_backend.Infrastructure.Data;
using Microsoft.EntityFrameworkCore;
using orienteering_backend.Core.Domain.Checkpoint.Events;

// Lisens MediatR: https://github.com/jbogard/MediatR/blob/master/LICENSE

namespace orienteering_backend.Core.Domain.Quiz.Handlers;

public class CheckpointDeletedHandler : INotificationHandler<CheckpointDeleted>
{
    private readonly OrienteeringContext _db;
    public CheckpointDeletedHandler(OrienteeringContext db)
    {
        _db = db ?? throw new ArgumentNullException(nameof(db));

    }

    public async Task Handle(CheckpointDeleted notification, CancellationToken cancellationToken)
    {
        var quiz = await _db.Quiz
            .Where(q => q.Id == notification.QuizId)
            .FirstOrDefaultAsync(cancellationToken);


        //if quiz is null, the checkpoint did not have quiz
        //else delete the quiz
        if (quiz != null)
        {
            _db.Quiz.Remove(quiz);  
            await _db.SaveChangesAsync(cancellationToken);
        }

    }
}
