using MediatR;
using Microsoft.EntityFrameworkCore;
using Microsoft.Extensions.Hosting;
using orienteering_backend.Core.Domain.Checkpoint;
using orienteering_backend.Core.Domain.Checkpoint.Dto;
using orienteering_backend.Core.Domain.Checkpoint.Pipelines;
using orienteering_backend.Infrastructure.Data;
using System;
// Kilder: https://thecodeblogger.com/2021/07/07/in-memory-database-provider-for-testing-net-ef-core-app/ (17.02.2023)

namespace orienteering_backend.Tests.Helpers;
public class InMemoryTest
{
    public readonly DbContextOptions<OrienteeringContext> dbContextOptions;

    public InMemoryTest()
    {
        // Build DbContextOptions
        dbContextOptions = new DbContextOptionsBuilder<OrienteeringContext>()
            .UseInMemoryDatabase(databaseName: "orienteeringTest")
            .Options;
    }

    //[Fact]

    //public async Task CreateCheckpointTest()
    //{
    //    //var inMemoryTest = new InMemoryTest();
    //    var _db = new OrienteeringContext(dbContextOptions, null);
    //    if (!_db.Database.IsInMemory())
    //    {
    //        _db.Database.Migrate();
    //    }


    //    var trackId = new Guid();
    //    Checkpoint checkpoint = new("test1", 0, trackId);
    //    CheckpointDto checkpointDto = new("test1", trackId, 0);

    //    var request = new CreateCheckpoint.Request(checkpointDto);
    //    var handler = new CreateCheckpoint.Handler(_db);
    //    var checkpointId = handler.Handle(request, CancellationToken.None).GetAwaiter().GetResult();
    //    var result = await _db.Checkpoints.FirstOrDefaultAsync(c => c.Id == checkpointId);
    //    Assert.Equal(checkpoint, result);
    //}

    [Fact]

    public async Task getSingleCheckpointTest()
    {
        //var inMemoryTest = new InMemoryTest();
        var _db = new OrienteeringContext(dbContextOptions, null);
        if (!_db.Database.IsInMemory())
        {
            _db.Database.Migrate();
        }


        var trackId = Guid.NewGuid();
        var checkpoint = new Checkpoint("test1", 0, trackId);
        CheckpointDto checkpointDto = new("test1", trackId, 0);
        await _db.Checkpoints.AddAsync(checkpoint);
        await _db.SaveChangesAsync();
        var request = new GetSingleCheckpoint.Request(checkpoint.Id);
        var handler = new GetSingleCheckpoint.Handler(_db);
        var returnedDto = handler.Handle(request, CancellationToken.None).GetAwaiter().GetResult();
        Assert.Equal(checkpointDto.Title, returnedDto.Title);
        Assert.Equal(checkpointDto.TrackId, returnedDto.TrackId);
        Assert.Equal(checkpointDto.GameId, returnedDto.GameId);

    }

    [Fact]

    public async Task test1()
    {
        var a = 1;
        var b = a; 
        Assert.Equal(a, b);
    }
}
