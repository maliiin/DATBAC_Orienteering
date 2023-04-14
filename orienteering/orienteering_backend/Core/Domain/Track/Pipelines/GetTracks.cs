using MediatR;
using Microsoft.EntityFrameworkCore;
using orienteering_backend.Infrastructure.Data;
using orienteering_backend.Core.Domain.Track.Dto;
using AutoMapper;
using System.Security.Authentication;
using orienteering_backend.Core.Domain.Authentication.Services;

namespace orienteering_backend.Core.Domain.Track.Pipelines;

public static class GetTracks
{
    public record Request() : IRequest<List<TrackDto>>;
    //Guid UserId) : IRequest<List<Track>>;


    public class Handler : IRequestHandler<Request, List<TrackDto>>
    //public class Handler : IRequestHandler<Request, List<Track>>
    {
        private readonly OrienteeringContext _db;
        private readonly IMapper _mapper;
        private readonly IIdentityService _identityService;

        public Handler(OrienteeringContext db, IMapper mapper, IIdentityService identityService)
        {
            _db = db ?? throw new ArgumentNullException(nameof(db));
            _mapper = mapper;
            _identityService = identityService;
        }

        public async Task<List<TrackDto>> Handle(Request request, CancellationToken cancellationToken)
       // public async Task<List<Track>> Handle(Request request, CancellationToken cancellationToken)
        {
            //check that signed in
            var userId = _identityService.GetCurrentUserId();
            if (userId == null) { throw new AuthenticationException("user not signed in"); }


            var tracks = await _db.Tracks
                .Where(t => t.UserId == userId)
                .ToArrayAsync(cancellationToken);//ToListAsync();

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
