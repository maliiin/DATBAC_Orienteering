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

public static class GetTrackUserByCheckpoint
{
    public record Request(
        Guid trackId) : IRequest<TrackUserIdDto>;
    //Guid UserId) : IRequest<List<Track>>;


    public class Handler : IRequestHandler<Request, TrackUserIdDto>
    //public class Handler : IRequestHandler<Request, List<Track>>
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

            //fix error hvis null
            if (track == null) { return null; }

            //create dto
            TrackUserIdDto trackDto = _mapper.Map<TrackUserIdDto>(track);

            return trackDto;
        }
    }

}
