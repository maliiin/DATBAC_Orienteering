using MediatR;
using Microsoft.EntityFrameworkCore;
using orienteering_backend.Infrastructure.Data;
using orienteering_backend.Core.Domain.Track.Dto;
using AutoMapper;
using System.Security.Authentication;
using orienteering_backend.Core.Domain.Authentication.Services;

// License MediatR (Apache 2.0): https://github.com/jbogard/MediatR/blob/master/LICENSE
// License Automapper (MIT): https://github.com/AutoMapper/AutoMapper/blob/master/LICENSE.txt
// License .NET EFCore (MIT) https://github.com/dotnet/efcore/blob/main/LICENSE.txt

namespace orienteering_backend.Core.Domain.Track.Pipelines;

public static class GetTracks
{
    public record Request() : IRequest<List<TrackDto>>;


    public class Handler : IRequestHandler<Request, List<TrackDto>>
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
        {
            //check that signed in
            var userId = _identityService.GetCurrentUserId();
            if (userId == null) { throw new AuthenticationException("user not signed in"); }


            var tracks = await _db.Tracks
                .Where(t => t.UserId == userId)
                .ToArrayAsync(cancellationToken);

            var trackDtoList = new List<TrackDto>();
            for (var i=0; i < tracks.Length; i++)
            {
                var track = tracks[i];
                var trackDto = _mapper.Map<Track, TrackDto>(track);
                trackDtoList.Add(trackDto);
            }
            return trackDtoList;
        }
    }

}
