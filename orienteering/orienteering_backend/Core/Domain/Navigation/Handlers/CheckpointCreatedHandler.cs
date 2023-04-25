using MediatR;
using orienteering_backend.Core.Domain.Checkpoint.Events;
using orienteering_backend.Infrastructure.Data;

// License MediatR (Apache 2.0): https://github.com/jbogard/MediatR/blob/master/LICENSE

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
            //create navigation
            var navigation = new Navigation(notification.CheckpointId);
            await _db.Navigation.AddAsync(navigation, cancellationToken);
            await _db.SaveChangesAsync(cancellationToken);

            //create wwwroot/checkpointId directory
            string folder = $"{notification.CheckpointId}";
            string dirPath = Path.Combine(Directory.GetCurrentDirectory(), "wwwroot", folder);
            Directory.CreateDirectory(dirPath);


        }
    }
}