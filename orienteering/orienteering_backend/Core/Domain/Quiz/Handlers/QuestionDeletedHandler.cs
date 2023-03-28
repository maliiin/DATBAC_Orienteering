
using MediatR;
using System;
using orienteering_backend.Infrastructure.Data;
using Microsoft.EntityFrameworkCore;
using orienteering_backend.Core.Domain.Checkpoint.Pipelines;
using orienteering_backend.Core.Domain.Checkpoint.Events;
using orienteering_backend.Core.Domain.Quiz.Events;

namespace orienteering_backend.Core.Domain.Quiz.Handlers;

public class QuestionDeletedHandler : INotificationHandler<QuizQuestionDeleted>
{
    private readonly OrienteeringContext _db;
    private readonly IMediator _mediator;
    public QuestionDeletedHandler(OrienteeringContext db, IMediator mediator)
    {
        _db = db ?? throw new ArgumentNullException(nameof(db));
        _mediator = mediator;

    }
    public async Task Handle(QuizQuestionDeleted notification, CancellationToken cancellationToken)
    {
        //fix-dette bør kanskje være pipeline heller? så slipper vi at event og handler er i samme domain??

       

        var quiz = await _db.Quiz
            .Where(q => q.Id == notification.QuizId)
            .Include(q=>q.QuizQuestions)
            .ThenInclude(q=>q.Alternatives)
            .FirstOrDefaultAsync(cancellationToken);

        //fix error 
        if (quiz == null) { return; }

        quiz.RemoveQuizQuestion(notification.QuestionId);
        //delete
        //_db.Quiz.Remove(quiz);
        //fix trengs denne?
        await _db.SaveChangesAsync(cancellationToken);


    }
}
