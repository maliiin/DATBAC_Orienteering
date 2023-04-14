using MediatR;
using orienteering_backend.Infrastructure.Data;
using Microsoft.EntityFrameworkCore;
using orienteering_backend.Core.Domain.Checkpoint.Events;

// Lisens MediatR: https://github.com/jbogard/MediatR/blob/master/LICENSE

namespace orienteering_backend.Core.Domain.Track.Handlers;


//oppgave- kall removed checkpoint på track så telleren minker

//fix-heter 1 fordi den andre kanskje ikke trengs?
//hvis ikke kan de slås sammen??
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



        //fix-sjekk om rett- her er det ingen error handling. (og skal ikke være det heller?)
        //dersom track er null kan det være fordi eventet sendes ut når track slettes




    }
}
