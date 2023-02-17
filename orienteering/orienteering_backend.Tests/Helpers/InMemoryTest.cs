using MediatR;
using Microsoft.EntityFrameworkCore;
using orienteering_backend.Core.Domain.Checkpoint;
using orienteering_backend.Core.Domain.Checkpoint.Dto;
using orienteering_backend.Core.Domain.Checkpoint.Pipelines;
using orienteering_backend.Infrastructure.Data;
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

    
}