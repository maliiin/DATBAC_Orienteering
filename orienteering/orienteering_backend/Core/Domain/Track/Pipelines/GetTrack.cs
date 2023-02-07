using MediatR;
using Microsoft.EntityFrameworkCore;
using orienteering_backend.Infrastructure.Data;
//Kilder: CampusEats lab fra dat240

namespace orienteering_backend.Core.Domain.Track.Pipelines;

public static class GetTrack
{
    public record Request(
        Guid UserId) : IRequest<List<Track>>;


    public class Handler : IRequestHandler<Request, List<Track>>
    {
        private readonly OrienteeringContext _db;

        public Handler(OrienteeringContext db) => _db = db ?? throw new ArgumentNullException(nameof(db));

        public async Task<List<Track>> Handle(Request request, CancellationToken cancellationToken)
        {
            var tracks = await _db.Tracks.Where(t => t.UserId == request.UserId).Include(t => t.CheckpointList).ToListAsync();
            return tracks;
        }
    }

}
