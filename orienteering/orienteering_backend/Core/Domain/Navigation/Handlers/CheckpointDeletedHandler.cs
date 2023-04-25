using MediatR;
using Microsoft.EntityFrameworkCore;
using orienteering_backend.Core.Domain.Checkpoint.Events;
using orienteering_backend.Infrastructure.Data;

// License MediatR (Apache 2.0): https://github.com/jbogard/MediatR/blob/master/LICENSE
// License .NET EFCore (MIT) https://github.com/dotnet/efcore/blob/main/LICENSE.txt

namespace orienteering_backend.Core.Domain.Navigation.Handlers
{
    public class CheckpointDeletedHandler : INotificationHandler<CheckpointDeleted>
    {
        private readonly OrienteeringContext _db;
        private readonly IMediator _mediator;
        public CheckpointDeletedHandler(OrienteeringContext db, IMediator mediator)
        {
            _db = db ?? throw new ArgumentNullException(nameof(db));
            _mediator = mediator;
        }

        //remove nav of deleted checkpoint
        public async Task Handle(CheckpointDeleted notification, CancellationToken cancellationToken)
        {
            //get nav to delete
            var nav = await _db.Navigation
                .Where(n => n.ToCheckpoint == notification.CheckpointId)
                .Include(n => n.Images)
                .FirstOrDefaultAsync(cancellationToken);

            if (nav != null)
            {
                _db.Navigation.Remove(nav);
                await _db.SaveChangesAsync(cancellationToken);

                //delete folder
                string dirPath = Path.Combine(Directory.GetCurrentDirectory(), "wwwroot",nav.ToCheckpoint.ToString());
                Directory.Delete(dirPath, true);
            }
        }
    }
}