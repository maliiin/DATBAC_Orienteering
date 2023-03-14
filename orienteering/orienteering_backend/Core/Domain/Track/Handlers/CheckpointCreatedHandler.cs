using MediatR;
using System;
using orienteering_backend.Infrastructure.Data;
using Microsoft.EntityFrameworkCore;
using orienteering_backend.Core.Domain.Checkpoint.Pipelines;
using orienteering_backend.Core.Domain.Checkpoint.Events;

namespace orienteering_backend.Core.Domain.Track.Handlers;
//Kilder: CampusEats Handlers
//Kilder: https://github.com/dat240-2022/assignments/blob/main/Lab3/UiS.Dat240.Lab3/Core/Domain/Cart/Handlers/FoodItemNameChangedHandler.cs (07.02.2023)
// bruker samme struktur som kilden

public class CheckpointCreatedHandler : INotificationHandler<CheckpointCreated>
{
    private readonly OrienteeringContext _db;
    private readonly IMediator _mediator;
    public CheckpointCreatedHandler(OrienteeringContext db, IMediator mediator)
    {
        _db = db ?? throw new ArgumentNullException(nameof(db));
        _mediator = mediator;

    }
    public async Task Handle(CheckpointCreated notification, CancellationToken cancellationToken)
    {
        //fix-hva gjør denne koden?? hvorfor skjer ikke dette direkte i checkpoint domain?
        //(er der det kommer fra!!)
        var checkpoint = await _db.Checkpoints.SingleOrDefaultAsync(c => c.Id == notification.CheckpointId);
        if (checkpoint == null)
        {
            return;
        }
        await _mediator.Send(new GenerateQR.Request(notification.CheckpointId));


    }
}
