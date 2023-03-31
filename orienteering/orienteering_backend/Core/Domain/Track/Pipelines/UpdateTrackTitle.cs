using AutoMapper;
using MediatR;
using Microsoft.EntityFrameworkCore;
using orienteering_backend.Core.Domain.Authentication.Services;
using orienteering_backend.Core.Domain.Track.Dto;
using orienteering_backend.Infrastructure.Data;
using System.Security.Authentication;

namespace orienteering_backend.Core.Domain.Track.Pipelines;

public static class UpdateTrackTitle
{
    public record Request(
       Guid trackId, string newTitle) : IRequest<bool>;


    public class Handler : IRequestHandler<Request, bool>
    {
        private readonly OrienteeringContext _db;
        private readonly IIdentityService _identityService;

        public Handler(OrienteeringContext db, IIdentityService identityService)
        {
            _db = db ?? throw new ArgumentNullException(nameof(db));
            _identityService = identityService;
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

            track.Name = request.newTitle;

            await _db.SaveChangesAsync(cancellationToken);

            return true;
           
        }
    }

}
