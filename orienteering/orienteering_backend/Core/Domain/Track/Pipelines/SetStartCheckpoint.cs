using MediatR;
using Microsoft.EntityFrameworkCore;
using orienteering_backend.Infrastructure.Data;

// Lisens MediatR: https://github.com/jbogard/MediatR/blob/master/LICENSE

namespace orienteering_backend.Core.Domain.Track.Pipelines;
//fix ikke i bruk foreløpig!!
public static class SetStartCheckpoint
{
    public record Request(
       Guid trackId, string newTitle) : IRequest<bool>;


    public class Handler : IRequestHandler<Request, bool>
    {
        private readonly OrienteeringContext _db;


        public Handler(OrienteeringContext db)
        {
            _db = db ?? throw new ArgumentNullException(nameof(db));

        }

        public async Task<bool> Handle(Request request, CancellationToken cancellationToken)
        {

            var track = await _db.Tracks
                .Where(t => t.Id == request.trackId)
                .FirstOrDefaultAsync(cancellationToken);

            //fix errorhandling? eller ikke
            if (track == null) { return false; }
            track.Name = request.newTitle;

            await _db.SaveChangesAsync(cancellationToken);

            return true;

        }
    }

}
