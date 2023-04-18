using MediatR;

// Lisens MediatR: https://github.com/jbogard/MediatR/blob/master/LICENSE

namespace orienteering_backend.Core.Domain.Track.Events;


public record TrackDeleted : INotification
{
    public TrackDeleted(Guid trackId)
    {
        TrackId = trackId;
    }

    public Guid TrackId { get; }
}
