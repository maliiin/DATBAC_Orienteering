using MediatR;
using orienteering_backend.Infrastructure.Data;
using Microsoft.EntityFrameworkCore;
using orienteering_backend.Core.Domain.Checkpoint.Events;

// License MediatR (Apache 2.0): https://github.com/jbogard/MediatR/blob/master/LICENSE
// License .NET EFCore (MIT) https://github.com/dotnet/efcore/blob/main/LICENSE.txt

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
