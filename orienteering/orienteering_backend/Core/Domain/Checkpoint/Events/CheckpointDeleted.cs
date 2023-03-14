using MediatR;
using orienteering_backend.SharedKernel;


namespace orienteering_backend.Core.Domain.Checkpoint.Events;


public record CheckpointDeleted : IDomainEvent
{
    public CheckpointDeleted(Guid trackId)
    {
        TrackId = trackId;
    }

    public Guid TrackId { get; }

}
