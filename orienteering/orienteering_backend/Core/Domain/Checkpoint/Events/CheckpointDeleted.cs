using MediatR;
using orienteering_backend.SharedKernel;


namespace orienteering_backend.Core.Domain.Checkpoint.Events;


public record CheckpointDeleted : IDomainEvent
{
    public CheckpointDeleted(Guid trackId, Guid checkpointId)
    {
        TrackId = trackId;
        CheckpointId = checkpointId;
    }

    public Guid TrackId { get; }
    public Guid CheckpointId { get; }


}
