using MediatR;
using System;
using orienteering_backend.Infrastructure.Data;
using Microsoft.EntityFrameworkCore;
using orienteering_backend.Core.Domain.Checkpoint.Pipelines;
using orienteering_backend.Core.Domain.Checkpoint.Events;

namespace orienteering_backend.Core.Domain.Track.Handlers;


//oppgave- kall removed checkpoint på track så telleren minker

//fix-heter 1 fordi den andre kanskje ikke trengs?
//hvis ikke kan de slås sammen??
public class CheckpointDeletedHandler : INotificationHandler<CheckpointDeleted>
{
    private readonly OrienteeringContext _db;
    private readonly IMediator _mediator;
    public CheckpointDeletedHandler(OrienteeringContext db, IMediator mediator)
    {
        _db = db ?? throw new ArgumentNullException(nameof(db));
        _mediator = mediator;

    }
    public async Task Handle(CheckpointDeleted notification, CancellationToken cancellationToken)
    {
        //get track from db
        var track = await _db.Tracks
            .Where(t => t.Id == notification.TrackId)
            .FirstOrDefaultAsync(cancellationToken);

        //fix error?
        if (track == null) { throw new KeyNotFoundException("could not find track"); }


        Console.WriteLine("deleted checkpoint handler event");

        //checkpoint was added earlier
        track.RemovedCheckpoint();
        await _db.SaveChangesAsync(cancellationToken);
    }
}
