using AutoMapper;
using MediatR;
using Microsoft.EntityFrameworkCore;
using orienteering_backend.Core.Domain.Authentication.Services;
using orienteering_backend.Core.Domain.Track.Dto;
using orienteering_backend.Core.Domain.Track.Events;
using orienteering_backend.Infrastructure.Data;
using System.Security.Authentication;
//Kilder: CampusEats lab fra dat240

namespace orienteering_backend.Core.Domain.Track.Pipelines;
//fix fjern mapper fra funksjoner som ikke bruker den
public static class DeleteTrack
{
    public record Request(
        Guid trackId) : IRequest<bool>;


    public class Handler : IRequestHandler<Request, bool>
    {
        private readonly OrienteeringContext _db;
        private readonly IMapper _mapper;
        private readonly IIdentityService _identityService;
        private readonly IMediator _mediator;

        public Handler(OrienteeringContext db, IMapper mapper, IIdentityService identityService, IMediator mediator)
        {
            _db = db ?? throw new ArgumentNullException(nameof(db));
            _mapper = mapper;
            _identityService = identityService;
            _mediator = mediator;
        }

        public async Task<bool> Handle(Request request, CancellationToken cancellationToken)
        {
            //check that signed in
            var userId = _identityService.GetCurrentUserId();
            if (userId == null) { throw new AuthenticationException("user not signed in"); }

            var track = await _db.Tracks
                .Where(t => t.Id == request.trackId)
                .FirstOrDefaultAsync(cancellationToken);
            if (track == null) { return false; }

            //check that user is allowed to access track
            if (userId != track.UserId) { throw new AuthenticationException("user not allowed to access this"); }


            var id = track.Id;
             _db.Tracks.Remove(track);
            await _db.SaveChangesAsync(cancellationToken);
            


            //fix- send ut event her sånn at checkpoints blir slettet og
            await _mediator.Publish(new TrackDeleted(id), cancellationToken);

            return true;
        }
    }

}
