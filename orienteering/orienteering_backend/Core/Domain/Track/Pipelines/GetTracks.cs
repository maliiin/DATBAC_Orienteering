using MediatR;
using Microsoft.EntityFrameworkCore;
using orienteering_backend.Infrastructure.Data;
using orienteering_backend.Core.Domain.Track.Dto;
using AutoMapper;
//Kilder: CampusEats lab fra dat240
// Kilder: https://github.com/dat240-2022/assignments/blob/main/Lab3/UiS.Dat240.Lab3/Core/Domain/Cart/Pipelines/AddItem.cs (07.02.2023)
// Brukte samme struktur på pipelinen som i kilden

namespace orienteering_backend.Core.Domain.Track.Pipelines;

public static class GetTracks
{
    public record Request(
        Guid UserId) : IRequest<List<TrackDto>>;
    //Guid UserId) : IRequest<List<Track>>;


    public class Handler : IRequestHandler<Request, List<TrackDto>>
    //public class Handler : IRequestHandler<Request, List<Track>>
    {
        private readonly OrienteeringContext _db;

        private readonly IMapper _mapper;

        public Handler(OrienteeringContext db, IMapper mapper)
        {
            _db = db ?? throw new ArgumentNullException(nameof(db));
            _mapper = mapper ?? throw new ArgumentNullException(nameof(_mapper));

        }

        public async Task<List<TrackDto>> Handle(Request request, CancellationToken cancellationToken)
       // public async Task<List<Track>> Handle(Request request, CancellationToken cancellationToken)
        {

            var tracks = await _db.Tracks
                                         .Where(t => t.UserId == request.UserId)
                                         .ToArrayAsync(cancellationToken);//ToListAsync();
            //Console.WriteLine($"lengde inni {tracks.Count}");
            var trackDtoList = new List<TrackDto>();
            for (var i=0; i < tracks.Length; i++)
            {
                var track = tracks[i];
                //var dtoElement = new TrackDto(track.UserId, track.Name);
                //dtoElement.TrackId = track.Id;
                var trackDto = _mapper.Map<Track, TrackDto>(track);
                trackDtoList.Add(trackDto);
            }
            return trackDtoList;
        }
    }

}
