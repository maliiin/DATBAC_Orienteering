﻿using MediatR;
using Microsoft.EntityFrameworkCore;
using orienteering_backend.Core.Domain.Authentication.Services;
using orienteering_backend.Infrastructure.Data;
using System.Security.Authentication;

// License MediatR (Apache 2.0): https://github.com/jbogard/MediatR/blob/master/LICENSE
// License .NET EFCore (MIT) https://github.com/dotnet/efcore/blob/main/LICENSE.txt

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

            if (track == null) { throw new ArgumentNullException("not found or access not allowed"); }

            //check that user is allowed to access track
            if (userId != track.UserId) { throw new ArgumentNullException("not found or access not allowed"); }

            track.Name = request.newTitle;

            await _db.SaveChangesAsync(cancellationToken);

            return true;
           
        }
    }
}
