using AutoMapper;
using MediatR;
using Microsoft.EntityFrameworkCore;
using orienteering_backend.Core.Domain.Track;
using orienteering_backend.Core.Domain.Track.Dto;
using orienteering_backend.Infrastructure.Data;
//Kilder: CampusEats lab fra dat240
// Kilder: https://github.com/dat240-2022/assignments/blob/main/Lab3/UiS.Dat240.Lab3/Core/Domain/Cart/Pipelines/AddItem.cs (07.02.2023)
// Brukte samme struktur på pipelinen som i kilden

namespace orienteering_backend.Core.Domain.Track.Pipelines;

public static class GetSingleTrack
{
    public record Request(
        Guid trackId) : IRequest<TrackDto>;
    //Guid UserId) : IRequest<List<Track>>;


    public class Handler : IRequestHandler<Request, TrackDto>
    //public class Handler : IRequestHandler<Request, List<Track>>
    {
        private readonly OrienteeringContext _db;

        private readonly IMapper _mapper;

        public Handler(OrienteeringContext db, IMapper mapper)
        {
            _db = db ?? throw new ArgumentNullException(nameof(db));
            _mapper = mapper ?? throw new ArgumentNullException(nameof(_mapper));

        }

        public async Task<TrackDto> Handle(Request request, CancellationToken cancellationToken)
        // public async Task<List<Track>> Handle(Request request, CancellationToken cancellationToken)
        {
            var track = await _db.Tracks
                .Where(t => t.Id == request.trackId)
                .FirstOrDefaultAsync();

            //var tracks = await _db.Tracks
            //                             .Where(t => t.UserId == request.UserId)
            //                             .Include(t => t.CheckpointList)
            //                             .ToArrayAsync(cancellationToken);//ToListAsync();
            //Console.WriteLine($"lengde inni {tracks.Count}");
            if(track is null) { throw new NullReferenceException("track is null."); };
            //TrackDto trackDto = new(track.UserId, track.Name);
            //trackDto.TrackId = track.Id;
            var trackDto = _mapper.Map<Track, TrackDto>(track);
            return trackDto;
        }
    }

}
