﻿using MediatR;
using Microsoft.EntityFrameworkCore;
using orienteering_backend.Core.Domain.Checkpoint.Events;
using orienteering_backend.Core.Domain.Track;
using orienteering_backend.Core.Domain.Track.Events;
using orienteering_backend.Infrastructure.Data;
using System.Web.Mvc;

namespace orienteering_backend.Core.Domain.Checkpoint.Handlers
{
    public class TrackDeletedHandler : INotificationHandler<TrackDeleted>
    {
        private readonly OrienteeringContext _db;
        private readonly IMediator _mediator;
        public TrackDeletedHandler(OrienteeringContext db, IMediator mediator)
        {
            _db = db ?? throw new ArgumentNullException(nameof(db));
            _mediator = mediator;
        }

        //remove checkpoints of deleted track
        public async Task Handle(TrackDeleted notification, CancellationToken cancellationToken)
        {
            //get checkpoints to delete
            var checkpointList = await _db.Checkpoints
                .Where(c => c.TrackId == notification.TrackId)
                .ToListAsync(cancellationToken);

            foreach (var checkpoint in checkpointList)
            {
                // fix er dette rett? å publisere før lagring-hva om noe kræsjer?
                //eventuelt å lagre hver gang inni loopen
                _db.Checkpoints.Remove(checkpoint);
                await _mediator.Publish(new CheckpointDeleted(notification.TrackId, checkpoint.Id, checkpoint.QuizId));

            }
            await _db.SaveChangesAsync(cancellationToken);
        }
    }
}