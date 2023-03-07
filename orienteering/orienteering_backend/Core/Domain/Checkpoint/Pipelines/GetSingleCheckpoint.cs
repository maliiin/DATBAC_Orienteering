using MediatR;
using Microsoft.EntityFrameworkCore;
using orienteering_backend.Infrastructure.Data;
using orienteering_backend.Core.Domain.Checkpoint.Dto;
using AutoMapper;
//Kilder: CampusEats lab fra dat240
// Kilder: https://github.com/dat240-2022/assignments/blob/main/Lab3/UiS.Dat240.Lab3/Core/Domain/Cart/Pipelines/AddItem.cs (07.02.2023)
// Brukte samme struktur på pipelinen som i kilden

namespace orienteering_backend.Core.Domain.Checkpoint.Pipelines;

public static class GetSingleCheckpoint
{
    public record Request(
        Guid checkpointId) : IRequest<CheckpointDto>;
    //Guid UserId) : IRequest<List<Track>>;


    public class Handler : IRequestHandler<Request, CheckpointDto>
    //public class Handler : IRequestHandler<Request, List<Track>>
    {
        private readonly OrienteeringContext _db;
        private readonly IMapper _mapper;

        public Handler(OrienteeringContext db, IMapper mapper)
        {
            _db = db ?? throw new ArgumentNullException(nameof(db));
            _mapper = mapper;
        }

        public async Task<CheckpointDto> Handle(Request request, CancellationToken cancellationToken)
        // public async Task<List<Track>> Handle(Request request, CancellationToken cancellationToken)
        {
            var checkpoint = await _db.Checkpoints
                .Where(c => c.Id == request.checkpointId)
                .FirstOrDefaultAsync(cancellationToken);

            if (checkpoint is null) { throw new NullReferenceException("the checkpoint cannot be found"); };
            //var tracks = await _db.Tracks
            //                             .Where(t => t.UserId == request.UserId)
            //                             .Include(t => t.CheckpointList)
            //                             .ToArrayAsync(cancellationToken);//ToListAsync();
            //Console.WriteLine($"lengde inni {tracks.Count}");
            Console.WriteLine(checkpoint.GameId);
            //CheckpointDto checkpointDto = new(checkpoint.Title, checkpoint.TrackId, checkpoint.GameId);
            //checkpointDto.Id = checkpoint.Id;
            //checkpointDto.QuizId = checkpoint.QuizId;
            var checkpointDto = _mapper.Map<Checkpoint, CheckpointDto>(checkpoint);

            return checkpointDto;
        }
    }

}

