//fix-skriv lisens om mediatr!!!!
using AutoMapper;
using MediatR;
using orienteering_backend.Core.Domain.Authentication.Services;
using orienteering_backend.Core.Domain.Track.Dto;
using orienteering_backend.Infrastructure.Data;
using System.Security.Authentication;
//Kilder: CampusEats lab fra dat240

namespace orienteering_backend.Core.Domain.Track.Pipelines;

public static class CreateTrack
{
    public record Request(
        CreateTrackDto trackDto) : IRequest<Guid>;


    public class Handler : IRequestHandler<Request, Guid>
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

        public async Task<Guid> Handle(Request request, CancellationToken cancellationToken)
        {
            //check that signed in
            var userId = _identityService.GetCurrentUserId();
            if (userId == null) { throw new AuthenticationException("user not signed in"); }

            var newTrack = _mapper.Map<CreateTrackDto, Track>(request.trackDto);
            newTrack.UserId = userId;

            await _db.Tracks.AddAsync(newTrack, cancellationToken);
            await _db.SaveChangesAsync(cancellationToken);
            return newTrack.Id;
        }
    }
            
}
