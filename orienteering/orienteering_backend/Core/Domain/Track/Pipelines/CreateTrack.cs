using MediatR;
using Microsoft.EntityFrameworkCore;
using orienteering_backend.Core.Domain.Track.Dto;
using orienteering_backend.Infrastructure.Data;
//Kilder: CampusEats lab fra dat240

namespace orienteering_backend.Core.Domain.Track.Pipelines;

public static class CreateTrack
{
    public record Request(
        TrackDto trackDto) : IRequest<Guid>;


    public class Handler : IRequestHandler<Request, Guid>
    {
        private readonly OrienteeringContext _db;

        public Handler(OrienteeringContext db) => _db = db ?? throw new ArgumentNullException(nameof(db));

        public async Task<Guid> Handle(Request request, CancellationToken cancellationToken)
        {
            var newTrack = new Track(request.trackDto.UserId, request.trackDto.TrackName);
            await _db.Tracks.AddAsync(newTrack, cancellationToken);
            await _db.SaveChangesAsync(cancellationToken);
            return newTrack.Id;
        }
    }
            
}
