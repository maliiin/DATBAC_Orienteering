using AutoMapper;
using MediatR;
using Microsoft.EntityFrameworkCore;
using orienteering_backend.Core.Domain.Checkpoint.Dto;
using orienteering_backend.Infrastructure.Data;
//Kilder: CampusEats lab fra dat240
// Kilder: https://github.com/dat240-2022/assignments/blob/main/Lab3/UiS.Dat240.Lab3/Core/Domain/Cart/Pipelines/AddItem.cs (07.02.2023)
// Brukte samme struktur på pipelinen som i kilden

namespace orienteering_backend.Core.Domain.Checkpoint.Pipelines;

public static class GetQRCodes
{
    public record Request(
        Guid TrackId
        ) : IRequest<List<CheckpointNameAndQRCodeDto>>;


    public class Handler : IRequestHandler<Request, List<CheckpointNameAndQRCodeDto>>
    {
        private readonly OrienteeringContext _db;
        private readonly IMapper _mapper;


        public Handler(OrienteeringContext db, IMapper mapper)
        {
            _db = db ?? throw new ArgumentNullException(nameof(db));
            _mapper = mapper;
        }
        public async Task<List<CheckpointNameAndQRCodeDto>> Handle(Request request, CancellationToken cancellationToken)
        {


            var checkpointList = await _db.Checkpoints.Where(c => c.TrackId == request.TrackId).ToListAsync();
            if (checkpointList == null)
            {
                throw new Exception("Checkpoint not found");
            }
            var dtoList = new List<CheckpointNameAndQRCodeDto>();
            //if (trackOwner != request.UserId)
            //{
            //    return dtoList;
            //}
            //var checkpointList = await _db.Checkpoints.Where(c => checkpointListId.Contains(c.Id)).ToListAsync(cancellationToken);

            for (var i = 0; i < checkpointList.Count; i++)
            {
                var checkpoint = checkpointList[i];
                //var dtoElement = new CheckpointNameAndQRCodeDto();
                //dtoElement.Id = checkpoint.Id;
                //dtoElement.QRCode = checkpoint.QRCode;
                var dtoElement = _mapper.Map<Checkpoint, CheckpointNameAndQRCodeDto>(checkpoint);
                dtoList.Add(dtoElement);
            }
            return dtoList;
        }
    }

}
