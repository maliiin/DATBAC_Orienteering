using MediatR;
using Microsoft.EntityFrameworkCore;
using orienteering_backend.Infrastructure.Data;
//Kilder: CampusEats lab fra dat240
// Kilder: https://github.com/dat240-2022/assignments/blob/main/Lab3/UiS.Dat240.Lab3/Core/Domain/Cart/Pipelines/AddItem.cs (07.02.2023)
// Brukte samme struktur på pipelinen som i kilden

namespace orienteering_backend.Core.Domain.Track.Pipelines;

public static class GetTrack
{
    public record Request(
        Guid UserId) : IRequest<Array>;
    //Guid UserId) : IRequest<List<Track>>;


    public class Handler : IRequestHandler<Request, Array>
    //public class Handler : IRequestHandler<Request, List<Track>>
    {
        private readonly OrienteeringContext _db;

        public Handler(OrienteeringContext db) => _db = db ?? throw new ArgumentNullException(nameof(db));

        public async Task<Array> Handle(Request request, CancellationToken cancellationToken)
       // public async Task<List<Track>> Handle(Request request, CancellationToken cancellationToken)
        {
            var tracks = await _db.Tracks
                                         .Where(t => t.UserId == request.UserId)
                                         .Include(t => t.CheckpointList)
                                         .ToArrayAsync(cancellationToken);//ToListAsync();
            //Console.WriteLine($"lengde inni {tracks.Count}");
            return tracks;
        }
    }

}
