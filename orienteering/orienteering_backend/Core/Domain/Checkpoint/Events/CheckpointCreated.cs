using orienteering_backend.SharedKernel;

namespace orienteering_backend.Core.Domain.Checkpoint.Events;


public record CheckpointCreated : IDomainEvent
{
    public CheckpointCreated(Guid checkpointId, Guid trackId)
    {
        CheckpointId = checkpointId;
        TrackId=trackId;
    }

    public Guid CheckpointId { get; }
    public Guid TrackId { get; }

}
