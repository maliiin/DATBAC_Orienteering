using MediatR;
using System;
using orienteering_backend.Infrastructure.Data;
using Microsoft.EntityFrameworkCore;
using orienteering_backend.Core.Domain.Checkpoint.Pipelines;
using orienteering_backend.Core.Domain.Checkpoint.Events;

namespace orienteering_backend.Core.Domain.Track.Handlers;


//oppgave- kall added checkpoint på track så telleren øker

//fix-heter 1 fordi den andre kanskje ikke trengs?
//hvis ikke kan de slås sammen??
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
        //get track from db
        var track = await _db.Tracks
            .Where(t => t.Id == notification.TrackId)
            .FirstOrDefaultAsync(cancellationToken);

        //fix error?
        if (track == null) { throw new KeyNotFoundException("could not find track"); }

        //checkpoint was added earlier
        track.AddedCheckpoint();
        await _db.SaveChangesAsync(cancellationToken);
    }
}
