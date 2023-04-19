using MediatR;
using orienteering_backend.Infrastructure.Data;
using Microsoft.EntityFrameworkCore;
using orienteering_backend.Core.Domain.Checkpoint.Events;

// Lisens MediatR: https://github.com/jbogard/MediatR/blob/master/LICENSE

namespace orienteering_backend.Core.Domain.Track.Handlers;

//This pipeline calls addedCheckpoint() on track which decrements the NumCheckpoints counter

public class CheckpointCreatedHandler : INotificationHandler<CheckpointCreated>
{
    private readonly OrienteeringContext _db;
    public CheckpointCreatedHandler(OrienteeringContext db)
    {
        _db = db ?? throw new ArgumentNullException(nameof(db));

    }
    public async Task Handle(CheckpointCreated notification, CancellationToken cancellationToken)
    {
        //get track from db
        var track = await _db.Tracks
            .Where(t => t.Id == notification.TrackId)
            .FirstOrDefaultAsync(cancellationToken);

        if (track == null) { throw new ArgumentNullException("could not find track"); }

        //checkpoint was added earlier
        track.AddedCheckpoint();
        await _db.SaveChangesAsync(cancellationToken);
    }
}
