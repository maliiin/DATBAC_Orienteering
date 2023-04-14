
using MediatR;
using orienteering_backend.Infrastructure.Data;
using orienteering_backend.Core.Domain.Checkpoint.Events;

// Lisens MediatR: https://github.com/jbogard/MediatR/blob/master/LICENSE

namespace orienteering_backend.Core.Domain.Quiz.Handlers;



public class CheckpointCreatedHandler : INotificationHandler<QuizCheckpointCreated>
{
    private readonly OrienteeringContext _db;
    private readonly IMediator _mediator;
    public CheckpointCreatedHandler(OrienteeringContext db, IMediator mediator)
    {
        _db = db ?? throw new ArgumentNullException(nameof(db));
        _mediator = mediator;

    }
    public async Task Handle(QuizCheckpointCreated notification, CancellationToken cancellationToken)
    {

        Quiz Quiz = new(notification.QuizId);

        _db.Quiz.Add(Quiz);
        await _db.SaveChangesAsync(cancellationToken);

        //fix!!!-brudd på ddd? ikke hent ut checkpoint fra db når du er i annet domain
        //checkpoint is created
        //var checkpoint = await _db.Checkpoints.SingleOrDefaultAsync(c => c.Id == notification.CheckpointId);
        //if (checkpoint == null)
        //{
        //    return;
        //}
        //await _mediator.Send(new GenerateQR.Request(notification.CheckpointId));


    }
}
