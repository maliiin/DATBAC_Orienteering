using MediatR;
using Microsoft.EntityFrameworkCore;
using orienteering_backend.Core.Domain.Checkpoint.Dto;
using orienteering_backend.Core.Domain.Checkpoint;
using orienteering_backend.Infrastructure.Data;
using orienteering_backend.Core.Domain.Checkpoint.Events;
using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Http.HttpResults;
using Microsoft.AspNetCore.Routing.Matching;
using AutoMapper;
using orienteering_backend.Core.Domain.Authentication.Services;
using System.Security.Authentication;
using orienteering_backend.Core.Domain.Track.Dto;
using orienteering_backend.Core.Domain.Track.Pipelines;
//Kilder: CampusEats lab fra dat240
// Kilder: https://github.com/dat240-2022/assignments/blob/main/Lab3/UiS.Dat240.Lab3/Core/Domain/Cart/Pipelines/AddItem.cs (07.02.2023)
// Brukte samme struktur på pipelinen som i kilden

namespace orienteering_backend.Core.Domain.Checkpoint.Pipelines;

public static class DeleteCheckpoint
{
    public record Request(
        Guid checkpointId) : IRequest<bool>;


    public class Handler : IRequestHandler<Request, bool>
    {
        private readonly OrienteeringContext _db;
        private readonly IIdentityService _identityService;
        private readonly IMediator _mediator;

        //public Handler(OrienteeringContext db) => _db = db ?? throw new ArgumentNullException(nameof(db));
        public Handler(OrienteeringContext db, IIdentityService identityService, IMediator mediator)
        {
            _db = db ?? throw new ArgumentNullException(nameof(db));
            _identityService = identityService;
            _mediator = mediator;
        }
        public async Task<bool> Handle(Request request, CancellationToken cancellationToken)
        {
            //fix returtype

            //check that signed in
            var userId = _identityService.GetCurrentUserId();
            if (userId == null) { throw new AuthenticationException("user not signed in"); }

            //get checkpoint to delete
            var checkpoint = await _db.Checkpoints
                .Where(ch => ch.Id == request.checkpointId)
                .FirstOrDefaultAsync(cancellationToken);

            if (checkpoint is null) { throw new NullReferenceException("the checkpoint cannot be found, or you cannot access it"); };

            //check that user is allowed to access this checkpoint
            TrackUserIdDto track = await _mediator.Send(new GetTrackUser.Request(checkpoint.TrackId));

            //return null even if it exist because dont tell atacker that it exists!
            if (userId != track.UserId) { throw new NullReferenceException("the checkpoint cannot be found, or you cannot access it"); }


            //delete
            _db.Checkpoints.Remove(checkpoint);

            //get all checkpoints where order was higher than the deleted one
            var checkpointList = await _db.Checkpoints
                .Where(ch => ch.Order > checkpoint.Order)
                .ToListAsync(cancellationToken);

            //update order of all those checkpoints 
            foreach (var singleCheckpoint in checkpointList)
            {
                singleCheckpoint.Order -= 1;
            }

            await _db.SaveChangesAsync(cancellationToken);

            //send event
            await _mediator.Publish(new CheckpointDeleted(checkpoint.TrackId, request.checkpointId, checkpoint.QuizId));

            return true;

        }
    }

}
