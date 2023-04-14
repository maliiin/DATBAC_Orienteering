using AutoMapper;
using MediatR;
using Microsoft.EntityFrameworkCore;
using orienteering_backend.Core.Domain.Authentication.Services;
using orienteering_backend.Core.Domain.Track.Dto;
using orienteering_backend.Infrastructure.Data;
using System.Security.Authentication;

namespace orienteering_backend.Core.Domain.Track.Pipelines;

public static class GetSingleTrack
{
    public record Request(
        Guid trackId) : IRequest<TrackDto>;


    public class Handler : IRequestHandler<Request, TrackDto>
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

        public async Task<TrackDto> Handle(Request request, CancellationToken cancellationToken)
        {
            //check that signed in
            var userId = _identityService.GetCurrentUserId();
            if (userId == null) { throw new AuthenticationException("user not signed in"); }

            var track = await _db.Tracks
                .Where(t => t.Id == request.trackId)
                .FirstOrDefaultAsync();
            if (track == null) { throw new NullReferenceException("this track dont exist"); }

            //check that user is allowed to access track
            if (userId != track.UserId) { throw new AuthenticationException("user not allowed to access this"); }
            
            //create dto
            var trackDto = _mapper.Map<Track, TrackDto>(track);
            return trackDto;
        }
    }

}
