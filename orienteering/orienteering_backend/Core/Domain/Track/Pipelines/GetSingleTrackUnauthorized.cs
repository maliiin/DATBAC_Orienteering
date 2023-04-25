using AutoMapper;
using MediatR;
using Microsoft.EntityFrameworkCore;
using orienteering_backend.Core.Domain.Track.Dto;
using orienteering_backend.Infrastructure.Data;

// License MediatR (Apache 2.0): https://github.com/jbogard/MediatR/blob/master/LICENSE
// License Automapper (MIT): https://github.com/AutoMapper/AutoMapper/blob/master/LICENSE.txt
// License .NET EFCore (MIT) https://github.com/dotnet/efcore/blob/main/LICENSE.txt

namespace orienteering_backend.Core.Domain.Track.Pipelines;

public static class GetSingleTrackUnauthorized
{
    public record Request(
        Guid trackId) : IRequest<TrackDto>;

    public class Handler : IRequestHandler<Request, TrackDto>
    {
        private readonly OrienteeringContext _db;
        private readonly IMapper _mapper;

        public Handler(OrienteeringContext db, IMapper mapper)
        {
            _db = db ?? throw new ArgumentNullException(nameof(db));
            _mapper = mapper;
        }

        public async Task<TrackDto> Handle(Request request, CancellationToken cancellationToken)
        {
            var track = await _db.Tracks
                .Where(t => t.Id == request.trackId)
                .FirstOrDefaultAsync();
            if (track == null) { throw new ArgumentNullException("this track dont exist"); }

            //create dto
            var trackDto = _mapper.Map<Track, TrackDto>(track);
            return trackDto;
        }
    }

}
