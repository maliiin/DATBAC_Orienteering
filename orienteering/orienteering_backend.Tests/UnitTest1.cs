










//using orienteering_backend.Core.Domain.Authentication.Services;
//using Microsoft.AspNetCore.Mvc.Testing;
//using Microsoft.Extensions.DependencyInjection;
//using System.Net;
//using Xunit;
//using Microsoft.AspNetCore.Hosting;
//using orienteering_backend.Infrastructure.Data;
//using Microsoft.EntityFrameworkCore;

////fix rett namespace
//namespace orienteering_backend.Tests;
////public class FactoryTest : WebApplicationFactory<Program>
////{
////    protected override void ConfigureWebHost(IWebHostBuilder builder)
////    {
////        base.ConfigureWebHost(builder);
////        builder.ConfigureServices(services =>
////        {
////            var g = Guid.NewGuid().ToString();
////            var opt = services.FirstOrDefault(a => a.ServiceType == typeof(DbContextOptions<OrienteeringContext>));
////            if (opt != null)
////                services.Remove(opt);
////            services.AddDbContext<OrienteeringContext>(options =>
////            {
////                options.UseInMemoryDatabase("InMemoryDbForTesting" + g);
////            });

////        });
////    }
////    //protected override void ConfigureWebHost(IWebHostBuilder builder)
////    //{
////    //    base.ConfigureWebHost(builder);
////    //    builder.ConfigureServices(services =>
////    //    {
////    //        var g = Guid.NewGuid().ToString();
////    //        var opt = services.FirstOrDefault(a => a.ServiceType == typeof(DbContextOptions<GuessingGameDbContext>));
////    //        if (opt != null)
////    //            services.Remove(opt);
////    //        services.AddDbContext<GuessingGameDbContext>(options =>
////    //        {
////    //            options.UseInMemoryDatabase("InMemoryDbForTesting" + g);
////    //        });

////    //    });
////    //}
////}

//[Collection("Tests")]
//public class ApplicationTests : IClassFixture<WebApplicationFactory<Program>>
//{
//    private readonly WebApplicationFactory<Program> factory;

//    public ApplicationTests(FactoryTest factoryTest)
//    {
//        this.factory = factoryTest;
//    }

//    [Fact]
//    public void TestRandomImageID()
//    {
//        using var scope = factory.Services.CreateScope();
//        var oracleService = scope.ServiceProvider.GetService<IIdentityService>();


//        Assert.NotNull(oracleService);


//    }
//}

////[Collection("Tests")]
////public class ApplicationTests : IClassFixture<FactoryTest>
////{
////    private readonly WebApplicationFactory<Program> factory;

////    public ApplicationTests(FactoryTest factoryTest)
////    {
////        this.factory = factoryTest;
////    }

////    [Fact]
////    public void IIdentityServiceTest()
////    {
////        using var scope = factory.Services.CreateScope();
////        var identityService = scope.ServiceProvider.GetService<IIdentityService>();

////        Assert.NotNull(identityService);
////    }

////    //[Fact]
////    //public void IIdentityServiceTest()
////    //{
////    //    using var scope = factory.Server.Services.CreateScope();
////    //    var identityService = scope.ServiceProvider.GetService<IIdentityService>();

////    //    Assert.NotNull(identityService);
////    //}

////    //[Fact]
////    //public void IImageServiceTest()
////    //{

////    //    using var scope = factory.Server.Services.CreateScope();
////    //    var identityService = scope.ServiceProvider.GetService<IImageService>();

////    //    Assert.NotNull(identityService);
////    //}

////    //[Fact]
////    //public void TestRandomImageID()
////    //{
////    //    using var scope = factory.Services.CreateScope();
////    //    var oracleService = scope.ServiceProvider.GetService<IOracleService>();

////    //    if (oracleService != null)
////    //    {
////    //        Assert.NotNull(oracleService.GetRandomImageId("Other"));
////    //    }

////    //}

////}
//////namespace orienteering_backend.Tests
//////{
//////    public class UnitTest1
//////    {

//////        [Fact]
//////        public async Task CreateCheckpointTest()
//////        {
//////            var inMemoryTest = new InMemoryTest();
//////            var _db = new OrienteeringContext(inMemoryTest.dbContextOptions, null);
//////            if (!_db.Database.IsInMemory())
//////            {
//////                _db.Database.Migrate();
//////            }


//////            var trackId = new Guid();
//////            Checkpoint checkpoint = new("test1", 0, trackId);
//////            CheckpointDto checkpointDto = new("test1", trackId, 0);

//////            var request = new CreateCheckpoint.Request(checkpointDto);
//////            var handler = new CreateCheckpoint.Handler(_db, Mediator);
//////            var checkpointId = handler.Handle(request, CancellationToken.None).GetAwaiter().GetResult();
//////            var result = await _db.Checkpoints.FirstOrDefaultAsync(c => c.Id == checkpointId);
//////            Assert.Equal(checkpoint, result);
//////        }



//////    }
//////}