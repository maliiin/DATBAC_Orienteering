using Microsoft.EntityFrameworkCore;
using orienteering_backend.Core.Domain.Checkpoint.Dto;
using orienteering_backend.Core.Domain.Checkpoint.Pipelines;
using orienteering_backend.Core.Domain.Checkpoint;
using orienteering_backend.Infrastructure.Data;
using orienteering_backend.Tests.Helpers;

namespace orienteering_backend.Tests
{
    public class UnitTest1 : InMemoryTest
    {
        [Fact]
        public async Task CreateCheckpointTest()
        {
            //var inMemoryTest = new InMemoryTest();
            var _db = new OrienteeringContext(dbContextOptions, null);
            if (!_db.Database.IsInMemory())
            {
                _db.Database.Migrate();
            }

            var trackId = new Guid();
            Checkpoint checkpoint = new("test1", 0, trackId);
            CheckpointDto checkpointDto = new("test1", trackId, 0);

            var request = new CreateCheckpoint.Request(checkpointDto);
            var handler = new CreateCheckpoint.Handler(_db, null);
            var checkpointId = handler.Handle(request, CancellationToken.None).GetAwaiter().GetResult();
            var result = await _db.Checkpoints.FirstOrDefaultAsync(c => c.Id == checkpointId);
            Assert.Equal(checkpoint, result);
        }
    }
}