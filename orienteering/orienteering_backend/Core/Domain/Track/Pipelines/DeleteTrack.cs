using AutoMapper;
using MediatR;
using Microsoft.EntityFrameworkCore;
using orienteering_backend.Core.Domain.Track.Dto;
using orienteering_backend.Infrastructure.Data;
//Kilder: CampusEats lab fra dat240

namespace orienteering_backend.Core.Domain.Track.Pipelines;
//fix fjern mapper fra funksjoner som ikke bruker den
public static class DeleteTrack
{
    public record Request(
        Guid trackId) : IRequest<bool>;


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
            if (track == null) { return false; }
             _db.Tracks.Remove(track);
            await _db.SaveChangesAsync(cancellationToken);



            //fix- send ut event her sånn at checkpoints blir slettet og
            return true;
        }
    }

}
