
using MediatR;
using System;
using orienteering_backend.Infrastructure.Data;
using Microsoft.EntityFrameworkCore;
using orienteering_backend.Core.Domain.Checkpoint.Pipelines;
using orienteering_backend.Core.Domain.Checkpoint.Events;

namespace orienteering_backend.Core.Domain.Quiz.Handlers;


public class CheckpointDeletedHandler : INotificationHandler<CheckpointDeleted>
{
    private readonly OrienteeringContext _db;
    private readonly IMediator _mediator;
    public CheckpointDeletedHandler(OrienteeringContext db, IMediator mediator)
    {
        _db = db ?? throw new ArgumentNullException(nameof(db));
        _mediator = mediator;

    }

    //delete quiz because related checkpoint is deleted
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
