using MediatR;
using Microsoft.EntityFrameworkCore;
using orienteering_backend.Core.Domain.Track.Events;
using orienteering_backend.Infrastructure.Data;
//Kilder: CampusEats lab fra dat240
// Kilder: https://github.com/dat240-2022/assignments/blob/main/Lab3/UiS.Dat240.Lab3/Core/Domain/Cart/Pipelines/AddItem.cs (07.02.2023)
// Brukte samme struktur på pipelinen som i kilden

namespace orienteering_backend.Core.Domain.Track.Pipelines;

public static class CreateCheckpoint
{
    public record Request(
        Guid Trackid) : IRequest<Guid>;


    public class Handler : IRequestHandler<Request, Guid>
    {
        private readonly OrienteeringContext _db;
        private readonly IMediator _mediator;


        //public Handler(OrienteeringContext db) => _db = db ?? throw new ArgumentNullException(nameof(db));
        public Handler(OrienteeringContext db, IMediator mediator) {
            _db = db ?? throw new ArgumentNullException(nameof(db));
            _mediator = mediator;
            }
        public async Task<Guid> Handle(Request request, CancellationToken cancellationToken)
        {
            var newCheckpoint = new Checkpoint();
            //await _db.Checkpoints.AddAsync(newCheckpoint);
            var track = await _db.Tracks.FirstOrDefaultAsync(t => t.Id == request.Trackid);
            if (track != null)
            {
                track.AddCheckpoint(newCheckpoint);
            }
            else
            {
                //Fiks: exception eller noe slikt
            }
            await _db.SaveChangesAsync(cancellationToken);
            // publishing event 
            await _mediator.Publish(new CheckpointCreated(newCheckpoint.Id));
            //await _mediator.Send(new GenerateQR.Request(newCheckpoint.Id));

            await _db.SaveChangesAsync(cancellationToken);

            return newCheckpoint.Id;
        }
    }

}
