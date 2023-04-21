﻿using MediatR;
using Microsoft.EntityFrameworkCore;
using orienteering_backend.Infrastructure.Data;
using System.Security.Authentication;
using orienteering_backend.Core.Domain.Track.Dto;
using orienteering_backend.Core.Domain.Track.Pipelines;
using orienteering_backend.Core.Domain.Authentication.Services;
using orienteering_backend.Core.Domain.Checkpoint.Dto;

// Lisens MediatR: https://github.com/jbogard/MediatR/blob/master/LICENSE

namespace orienteering_backend.Core.Domain.Checkpoint.Pipelines;

public static class UpdateCheckpointDescription
{
    public record Request(
        CheckpointDescriptionDto checkpointInfo) : IRequest<Checkpoint>;

    public class Handler : IRequestHandler<Request, Checkpoint>
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


        public async Task<Checkpoint?> Handle(Request request, CancellationToken cancellationToken)
        {
            //check that signed in
            var userId = _identityService.GetCurrentUserId();
            if (userId == null) { throw new AuthenticationException("user not signed in"); }

            //get checkpoint
            var checkpoint = await _db.Checkpoints
                .Where(ch => ch.Id == request.checkpointInfo.CheckpointId)
                .FirstOrDefaultAsync(cancellationToken);

            if (checkpoint == null) { throw new ArgumentNullException(); };

            //check that user is allowed to access this checkpoint
            TrackUserIdDto track = await _mediator.Send(new GetTrackUser.Request(checkpoint.TrackId));
            if (userId != track.UserId) { throw new ArgumentNullException(); }

            //change title
            checkpoint.CheckpointDescription = request.checkpointInfo.Description;
            await _db.SaveChangesAsync(cancellationToken);
            return checkpoint;
        }
    }

}