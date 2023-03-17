using MediatR;
using orienteering_backend.Core.Domain.Checkpoint.Events;
using orienteering_backend.Core.Domain.Checkpoint.Pipelines;
using orienteering_backend.Core.Domain.Quiz.Events;
using orienteering_backend.Infrastructure.Data;

namespace orienteering_backend.Core.Domain.Navigation.Handlers
{
    public class CheckpointCreatedHandler : INotificationHandler<CheckpointCreated>
    {
        private readonly OrienteeringContext _db;
        private readonly IMediator _mediator;
        public CheckpointCreatedHandler(OrienteeringContext db, IMediator mediator)
        {
            _db = db ?? throw new ArgumentNullException(nameof(db));
            _mediator = mediator;

        }
        public async Task Handle(CheckpointCreated notification, CancellationToken cancellationToken)
        {
            var navigation = new Navigation(notification.CheckpointId);

            await _db.Navigation.AddAsync(navigation, cancellationToken);
            await _db.SaveChangesAsync(cancellationToken);


        }
    }
}