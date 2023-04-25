using MediatR;

// License MediatR (Apache 2.0): https://github.com/jbogard/MediatR/blob/master/LICENSE

namespace orienteering_backend.Core.Domain.Checkpoint.Events;


public record QuizCheckpointCreated : INotification
{
    public QuizCheckpointCreated(Guid checkpointId, Guid quizId)
    {
        CheckpointId = checkpointId;
        QuizId = quizId;

    }

    public Guid CheckpointId { get; }
    public Guid QuizId { get; }
}
