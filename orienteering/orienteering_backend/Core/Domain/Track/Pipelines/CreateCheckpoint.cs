using MediatR;
using Microsoft.EntityFrameworkCore;
using orienteering_backend.Infrastructure.Data;
//Kilder: CampusEats lab fra dat240

namespace orienteering_backend.Core.Domain.Track.Pipelines;

public static class CreateCheckpoint
{
    public record Request(
        Guid Trackid) : IRequest<int>;


    public class Handler : IRequestHandler<Request, int>
    {
        private readonly OrienteeringContext _db;

        public Handler(OrienteeringContext db) => _db = db ?? throw new ArgumentNullException(nameof(db));

        public async Task<int> Handle(Request request, CancellationToken cancellationToken)
        {
            var newCheckpoint = new Checkpoint();
            //await _db.Checkpoints.AddAsync(newCheckpoint);
            var track = await _db.Tracks.FirstOrDefaultAsync(t => t.Id == request.Trackid);
            if (track != null)
            {
                track.AddCheckpoint(newCheckpoint);
            }
            else
            {
                //Fiks: exception eller noe slikt
            }
            await _db.SaveChangesAsync(cancellationToken);

            return newCheckpoint.Id;
        }
    }

}
