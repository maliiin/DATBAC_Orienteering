using MediatR;
using orienteering_backend.SharedKernel;

namespace orienteering_backend.Core.Domain.Checkpoint.Events;


public record QuizCheckpointCreated : IDomainEvent
{
    public QuizCheckpointCreated(Guid checkpointId, Guid quizId)
    {
        CheckpointId = checkpointId;
        QuizId = quizId;

    }

    public Guid CheckpointId { get; }
    public Guid QuizId { get; }
}
