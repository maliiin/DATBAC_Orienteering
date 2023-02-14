using MediatR;
using Microsoft.EntityFrameworkCore;
using orienteering_backend.Core.Domain.Track.Events;
using orienteering_backend.Core.Domain.Track.Dto;
using orienteering_backend.Infrastructure.Data;
//Kilder: CampusEats lab fra dat240
// Kilder: https://github.com/dat240-2022/assignments/blob/main/Lab3/UiS.Dat240.Lab3/Core/Domain/Cart/Pipelines/AddItem.cs (07.02.2023)
// Brukte samme struktur på pipelinen som i kilden

namespace orienteering_backend.Core.Domain.Track.Pipelines;

public static class GetQRCodes
{
    public record Request(
        Guid TrackId
        ) : IRequest<List<CheckpointNameAndQRCodeDto>>;


    public class Handler : IRequestHandler<Request, List<CheckpointNameAndQRCodeDto>>
    {
        private readonly OrienteeringContext _db;
        private readonly IMediator _mediator;


        //public Handler(OrienteeringContext db) => _db = db ?? throw new ArgumentNullException(nameof(db));
        public Handler(OrienteeringContext db, IMediator mediator)
        {
            _db = db ?? throw new ArgumentNullException(nameof(db));
            _mediator = mediator;
        }
        public async Task<List<CheckpointNameAndQRCodeDto>> Handle(Request request, CancellationToken cancellationToken)
        {

            var track = await _db.Tracks.Include(t => t.CheckpointList).FirstOrDefaultAsync(t => t.Id == request.TrackId);
            //var trackOwner = track.UserId;
            var checkpointList = track.CheckpointList;
            var dtoList = new List<CheckpointNameAndQRCodeDto>();
            //if (trackOwner != request.UserId)
            //{
            //    return dtoList;
            //}
            for (var i=0; i<checkpointList.Count; i++)
            {
                var checkpoint = checkpointList[i];
                var dtoElement = new CheckpointNameAndQRCodeDto();
                dtoElement.Id = checkpoint.Id;
                dtoElement.QRCode =  checkpoint.QRCode;
                dtoList.Add(dtoElement);
            }
            return dtoList;
        }
    }

}
