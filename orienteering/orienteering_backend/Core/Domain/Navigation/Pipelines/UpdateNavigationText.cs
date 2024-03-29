﻿using MediatR;
using Microsoft.EntityFrameworkCore;
using orienteering_backend.Core.Domain.Authentication.Services;
using orienteering_backend.Core.Domain.Checkpoint.Dto;
using orienteering_backend.Core.Domain.Checkpoint.Pipelines;
using orienteering_backend.Core.Domain.Track.Dto;
using orienteering_backend.Core.Domain.Track.Pipelines;
using orienteering_backend.Infrastructure.Data;
using System.Security.Authentication;

// License MediatR (Apache 2.0): https://github.com/jbogard/MediatR/blob/master/LICENSE
// License .NET EFCore (MIT) https://github.com/dotnet/efcore/blob/main/LICENSE.txt

namespace orienteering_backend.Core.Domain.Navigation.Pipelines
{
    public class UpdateNavigationText
    {

        public record Request(
       Guid NavigationId, string newText, Guid NavigationImageId) : IRequest<bool>;


        public class Handler : IRequestHandler<Request, bool>
        {
            private readonly OrienteeringContext _db;
            private readonly IIdentityService _identityService;
            private readonly IMediator _mediator;

            public Handler(OrienteeringContext db, IIdentityService identityService, IMediator mediator)
            {
                _db = db ?? throw new ArgumentNullException(nameof(db));
                _identityService = identityService;
                _mediator = mediator;
            }
            public async Task<bool> Handle(Request request, CancellationToken cancellationToken)
            {
                //check that signed in
                var userId = _identityService.GetCurrentUserId();
                if (userId == null) { throw new AuthenticationException("user not signed in"); }

                //get navigation
                var navigation = await _db.Navigation
                    .Where(n => n.Id == request.NavigationId)
                    .Include(n => n.Images)
                    .FirstOrDefaultAsync(cancellationToken);

                if (navigation == null) { throw new ArgumentNullException("not found or access not allowed"); }

                //check that user is allowed to access this navigation
                CheckpointDto checkpoint = await _mediator.Send(new GetSingleCheckpoint.Request(navigation.ToCheckpoint));
                TrackUserIdDto track = await _mediator.Send(new GetTrackUser.Request(checkpoint.TrackId));

                if (userId != track.UserId) { throw new ArgumentNullException("not found or access not allowed"); }

                foreach (var navImage in navigation.Images)
                {
                    if (navImage.Id == request.NavigationImageId)
                    {
                        navImage.TextDescription = request.newText;
                        break;
                    }
                }
                await _db.SaveChangesAsync(cancellationToken);
                return true;
            }
        }
    }
}