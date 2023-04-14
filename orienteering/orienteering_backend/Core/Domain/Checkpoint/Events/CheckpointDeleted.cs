using orienteering_backend.SharedKernel;


namespace orienteering_backend.Core.Domain.Checkpoint.Events;


public record CheckpointDeleted : IDomainEvent
{
    public CheckpointDeleted(Guid trackId, Guid checkpointId, Guid? quizId)
    {
        TrackId = trackId;
        CheckpointId = checkpointId;
        QuizId = quizId;

    }

    public Guid TrackId { get; }
    public Guid CheckpointId { get; }
    public Guid? QuizId { get; }



}
