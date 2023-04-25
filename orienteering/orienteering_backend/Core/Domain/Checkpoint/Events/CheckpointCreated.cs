using MediatR;

// License MediatR (Apache 2.0): https://github.com/jbogard/MediatR/blob/master/LICENSE
// License MediatR (Apache 2.0): https://github.com/jbogard/MediatR/blob/master/LICENSE


namespace orienteering_backend.Core.Domain.Checkpoint.Events;


public record CheckpointCreated : INotification
{
    public CheckpointCreated(Guid checkpointId, Guid trackId)
    {
        CheckpointId = checkpointId;
        TrackId = trackId;
    }

    public Guid CheckpointId { get; }
    public Guid TrackId { get; }
    public Guid? QuizId { get; set; }
}
