using MediatR;
using Microsoft.EntityFrameworkCore;
using orienteering_backend.Core.Domain.Checkpoint.Dto;
using orienteering_backend.Core.Domain.Track;
using orienteering_backend.Infrastructure.Data;

namespace orienteering_backend.Core.Domain.Checkpoint.Pipelines
{

    public static class GetCheckpointsForTrack
    {
        public record Request(
            Guid trackId) : IRequest<List<CheckpointDto>>;


        public class Handler : IRequestHandler<Request, List<CheckpointDto>>
        {
            private readonly OrienteeringContext _db;
            private readonly IMediator _mediator;


            public Handler(OrienteeringContext db, IMediator mediator)
            {
                _db = db ?? throw new ArgumentNullException(nameof(db));
                _mediator = mediator;
            }
            public async Task<List<CheckpointDto>> Handle(Request request, CancellationToken cancellationToken)
            {

                var checkpointList = await _db.Checkpoints
                    .Where(c => c.TrackId == request.trackId)
                    .ToListAsync();

                //convert to dto
                var checkpointDtoList = new List<CheckpointDto>();
                for (var i = 0; i < checkpointList.Count; i++)
                {
                    var checkpoint = checkpointList[i];
                    var dtoElement = new CheckpointDto(checkpoint.Title, checkpoint.TrackId);
                    dtoElement.Id = checkpoint.Id;
                    checkpointDtoList.Add(dtoElement);
                }
                return checkpointDtoList;
            }
        }

    }
}
