using MediatR;

// License MediatR (Apache 2.0): https://github.com/jbogard/MediatR/blob/master/LICENSE

namespace orienteering_backend.Core.Domain.Checkpoint.Events;


public record CheckpointDeleted : INotification
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
