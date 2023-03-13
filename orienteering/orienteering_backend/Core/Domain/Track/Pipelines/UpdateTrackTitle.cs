using AutoMapper;
using MediatR;
using Microsoft.EntityFrameworkCore;
using orienteering_backend.Core.Domain.Track.Dto;
using orienteering_backend.Infrastructure.Data;

namespace orienteering_backend.Core.Domain.Track.Pipelines;

public static class UpdateTrackTitle
{
    public record Request(
       Guid trackId, string newTitle) : IRequest<bool>;


    public class Handler : IRequestHandler<Request, bool>
    {
        private readonly OrienteeringContext _db;

        private readonly IMapper _mapper;

        public Handler(OrienteeringContext db, IMapper mapper)
        {
            _db = db ?? throw new ArgumentNullException(nameof(db));
            _mapper = mapper ?? throw new ArgumentNullException(nameof(_mapper));

        }

        public async Task<bool> Handle(Request request, CancellationToken cancellationToken)
        {
            
            var track = await _db.Tracks
                .Where(t => t.Id == request.trackId)
                .FirstOrDefaultAsync(cancellationToken);

            if (track == null) { return false; }
            track.Name = request.newTitle;

            await _db.SaveChangesAsync(cancellationToken);

            return true;
           
        }
    }

}
