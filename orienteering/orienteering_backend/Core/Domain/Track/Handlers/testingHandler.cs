using MediatR;
using System;
using orienteering_backend.Core.Domain.Track.Events;
using orienteering_backend.Infrastructure.Data;
using orienteering_backend.Core.Domain.Track.Pipelines;
using Microsoft.EntityFrameworkCore;

namespace orienteering_backend.Core.Domain.QRCode.Handlers;
//Kilder: CampusEats Handlers
//Kilder: https://github.com/dat240-2022/assignments/blob/main/Lab3/UiS.Dat240.Lab3/Core/Domain/Cart/Handlers/FoodItemNameChangedHandler.cs (07.02.2023)
// bruker samme struktur som kilden

public class testingHandler : INotificationHandler<CheckpointCreated>
{
    private readonly OrienteeringContext _db;
    private readonly IMediator _mediator;
    public testingHandler(OrienteeringContext db, IMediator mediator)
    {
        _db = db ?? throw new ArgumentNullException(nameof(db));
        _mediator = mediator;

    }
    public async Task Handle(CheckpointCreated notification, CancellationToken cancellationToken)
    {
        Console.WriteLine("this is working perfectly");


    }
}
