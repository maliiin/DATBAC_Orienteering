using MediatR;
using Microsoft.EntityFrameworkCore;
using orienteering_backend.Infrastructure.Data;

namespace orienteering_backend.Core.Domain.Navigation.Pipelines
{
    public class UpdateNavigationText
    {

        public record Request(
       Guid NavigationId, string newText, Guid NavigationImageId) : IRequest<bool>;


        public class Handler : IRequestHandler<Request, bool>
        {
            private readonly OrienteeringContext _db;


            public Handler(OrienteeringContext db)
            {
                _db = db ?? throw new ArgumentNullException(nameof(db));
            }
            public async Task<bool> Handle(Request request, CancellationToken cancellationToken)
            {
                //get navigation
                var navigation = await _db.Navigation
                    .Where(n => n.Id == request.NavigationId)
                    .Include(n => n.Images)
                    .FirstOrDefaultAsync(cancellationToken);
                if (navigation == null) { return false; }

                foreach (var navImage in navigation.Images)
                {
                    if (navImage.Id == request.NavigationImageId)
                    {
                        navImage.TextDescription = request.newText;
                        break;
                    }
                }
                await _db.SaveChangesAsync(cancellationToken);
                return true;

                //navigation

                ////get checkpoint
                //var checkpoint = await _db.Checkpoints
                //    .Where(ch => ch.Id == request.checkpointId)
                //    .FirstOrDefaultAsync(cancellationToken);

                //if (checkpoint == null) { return null; };

                //checkpoint.Title = request.checkpointTitle;
                //await _db.SaveChangesAsync(cancellationToken);
                //return checkpoint;
            }
        }

    }
}