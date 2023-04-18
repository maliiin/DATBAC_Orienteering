using MediatR;
using orienteering_backend.Infrastructure.Data;
using Microsoft.EntityFrameworkCore;
using orienteering_backend.Core.Domain.Checkpoint.Events;

// Lisens MediatR: https://github.com/jbogard/MediatR/blob/master/LICENSE

namespace orienteering_backend.Core.Domain.Track.Handlers;


// This pipeline calls removedCheckpoint() on track which decrements the NumCheckpoints counter

public class CheckpointDeletedHandler : INotificationHandler<CheckpointDeleted>
{
    private readonly OrienteeringContext _db;
    public CheckpointDeletedHandler(OrienteeringContext db)
    {
        _db = db ?? throw new ArgumentNullException(nameof(db));

    }
    public async Task Handle(CheckpointDeleted notification, CancellationToken cancellationToken)
    {
        //get track from db
        var track = await _db.Tracks
            .Where(t => t.Id == notification.TrackId)
            .FirstOrDefaultAsync(cancellationToken);

        if (track != null) {

            //checkpoint was added earlier
            track.RemovedCheckpoint();
            await _db.SaveChangesAsync(cancellationToken);
        }
        //if track is null this event comes from track deleted, and nothing happens.

    }
}
