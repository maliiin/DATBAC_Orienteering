using MediatR;
using orienteering_backend.SharedKernel;


namespace orienteering_backend.Core.Domain.Track.Events;


public record TrackDeleted : IDomainEvent
{
    public TrackDeleted(Guid trackId)
    {
        TrackId = trackId;
    }

    public Guid TrackId { get; }
}
