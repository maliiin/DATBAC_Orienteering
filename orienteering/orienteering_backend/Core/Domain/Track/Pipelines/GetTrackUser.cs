using AutoMapper;
using MediatR;
using Microsoft.EntityFrameworkCore;
using orienteering_backend.Core.Domain.Track.Dto;
using orienteering_backend.Infrastructure.Data;

// License MediatR (Apache 2.0): https://github.com/jbogard/MediatR/blob/master/LICENSE

namespace orienteering_backend.Core.Domain.Track.Pipelines;

public static class GetTrackUser
{
    public record Request(
        Guid trackId) : IRequest<TrackUserIdDto>;

    public class Handler : IRequestHandler<Request, TrackUserIdDto>
    {
        private readonly OrienteeringContext _db;

        private readonly IMapper _mapper;

        public Handler(OrienteeringContext db, IMapper mapper)
        {
            _db = db ?? throw new ArgumentNullException(nameof(db));
            _mapper = mapper ?? throw new ArgumentNullException(nameof(_mapper));
        }

        public async Task<TrackUserIdDto> Handle(Request request, CancellationToken cancellationToken)
        {
            var track = await _db.Tracks
                .Where(t => t.Id == request.trackId)
                .FirstOrDefaultAsync();

            if (track == null) { throw new ArgumentNullException(); }

            //create dto
            TrackUserIdDto trackDto=_mapper.Map<Track,TrackUserIdDto>(track);

            return trackDto;
        }
    }

}
