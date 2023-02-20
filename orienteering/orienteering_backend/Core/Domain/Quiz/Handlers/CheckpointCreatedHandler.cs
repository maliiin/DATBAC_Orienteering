
using MediatR;
using System;
using orienteering_backend.Infrastructure.Data;
using Microsoft.EntityFrameworkCore;
using orienteering_backend.Core.Domain.Checkpoint.Pipelines;
using orienteering_backend.Core.Domain.Checkpoint.Events;

namespace orienteering_backend.Core.Domain.Quiz.Handlers;
//Kilder: CampusEats Handlers
//Kilder: https://github.com/dat240-2022/assignments/blob/main/Lab3/UiS.Dat240.Lab3/Core/Domain/Cart/Handlers/FoodItemNameChangedHandler.cs (07.02.2023)
// bruker samme struktur som kilden

public class CheckpointCreatedHandler : INotificationHandler<QuizCheckpointCreated>
{
    private readonly OrienteeringContext _db;
    private readonly IMediator _mediator;
    public CheckpointCreatedHandler(OrienteeringContext db, IMediator mediator)
    {
        _db = db ?? throw new ArgumentNullException(nameof(db));
        _mediator = mediator;

    }
    public async Task Handle(QuizCheckpointCreated notification, CancellationToken cancellationToken)
    {
        Console.WriteLine("handle checkpoint with quiz");
        Quiz Quiz = new(notification.QuizId);

        _db.Quiz.Add(Quiz);
        await _db.SaveChangesAsync(cancellationToken);

        //fix-brudd på ddd? ikke hent ut checkpoint fra db når du er i annet domain
        //checkpoint is created
        var checkpoint = await _db.Checkpoints.SingleOrDefaultAsync(c => c.Id == notification.CheckpointId);
        if (checkpoint == null)
        {
            return;
        }
        await _mediator.Send(new GenerateQR.Request(notification.CheckpointId));


    }
}
