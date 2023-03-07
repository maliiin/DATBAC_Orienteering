using AutoMapper;
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

        private readonly IMapper _mapper;

        public Handler(OrienteeringContext db, IMapper mapper)
        {
            _db = db ?? throw new ArgumentNullException(nameof(db));
            _mapper = mapper ?? throw new ArgumentNullException(nameof(_mapper));

        }

        public async Task<Guid> Handle(Request request, CancellationToken cancellationToken)
        {
            var newTrack = _mapper.Map<TrackDto, Track>(request.trackDto);
            await _db.Tracks.AddAsync(newTrack, cancellationToken);
            await _db.SaveChangesAsync(cancellationToken);
            return newTrack.Id;
        }
    }
            
}
