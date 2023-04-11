using MediatR;
using Microsoft.EntityFrameworkCore;
using orienteering_backend.Core.Domain.Checkpoint.Dto;
using orienteering_backend.Core.Domain.Checkpoint;
using orienteering_backend.Infrastructure.Data;
using orienteering_backend.Core.Domain.Checkpoint.Events;
using orienteering_backend.Core.Domain.Track.Pipelines;
using Microsoft.AspNetCore.Http.HttpResults;
using System.Security.Authentication;
//Kilder: CampusEats lab fra dat240
// Kilder: https://github.com/dat240-2022/assignments/blob/main/Lab3/UiS.Dat240.Lab3/Core/Domain/Cart/Pipelines/AddItem.cs (07.02.2023)
// Brukte samme struktur på pipelinen som i kilden

namespace orienteering_backend.Core.Domain.Checkpoint.Pipelines;

public static class CreateCheckpoint
{
    public record Request(
        CheckpointDto checkpointDto, Guid userId) : IRequest<Guid>;


    public class Handler : IRequestHandler<Request, Guid>
    {
        private readonly OrienteeringContext _db;
        private readonly IMediator _mediator;


        public Handler(OrienteeringContext db, IMediator mediator)
        {
            _db = db ?? throw new ArgumentNullException(nameof(db));
            _mediator = mediator;
        }
        public async Task<Guid> Handle(Request request, CancellationToken cancellationToken)
        {
            //get track to get numCheckpoint
            var trackDto=await _mediator.Send(new GetSingleTrack.Request(request.checkpointDto.TrackId));

            if (trackDto.UserId != request.userId) { throw new NullReferenceException("The user dont own this track or it dosent exist."); };

            //create checkpoint
            var newCheckpoint = new Checkpoint(request.checkpointDto.Title, request.checkpointDto.GameId, request.checkpointDto.TrackId);
            newCheckpoint.Order = trackDto.NumCheckpoints + 1;

            if (request.checkpointDto.GameId == 0)
            {
                //no game--> should be quiz
                newCheckpoint.QuizId= Guid.NewGuid();
            }

            await _db.Checkpoints.AddAsync(newCheckpoint);
            await _db.SaveChangesAsync(cancellationToken);

            // publishing event 
            await _mediator.Publish(new CheckpointCreated(newCheckpoint.Id, request.checkpointDto.TrackId));

            if (newCheckpoint.GameId == 0)
            {
                //checkpoint with quiz
                await _mediator.Publish(new QuizCheckpointCreated(newCheckpoint.Id, (Guid)newCheckpoint.QuizId));
            }

            //await _db.SaveChangesAsync(cancellationToken);

            return newCheckpoint.Id;
        }
    }

}
