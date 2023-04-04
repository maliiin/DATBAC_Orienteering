//using AutoMapper;
//using MediatR;
//using Microsoft.EntityFrameworkCore;
//using Moq;
//using orienteering_backend.Core.Domain.Authentication.Services;
//using orienteering_backend.Infrastructure.Automapper;
//using orienteering_backend.Infrastructure.Data;
//using System;
//using System.Collections.Generic;
//using System.Linq;
//using System.Text;
//using System.Threading.Tasks;
////fix-generelt på tester-er det best å bruke moq mapper eller _mapper?
////sjekk at vi er konsekvente
//namespace orienteering_backend.Tests.Helpers
//{
//    public class NavigationTest
//    {
//        public readonly DbContextOptions<OrienteeringContext> dbContextOptions;

//        private static IMapper _mapper;
//        public NavigationTest()
//        {
//            dbContextOptions = new DbContextOptionsBuilder<OrienteeringContext>()
//               .UseInMemoryDatabase(databaseName: "orienteeringTest")
//               .Options;

//            // "Mocker" automapper Fix bruker mock nå heller
//            //Kilder: https://www.thecodebuzz.com/unit-test-mock-automapper-asp-net-core-imapper/ (06.03.2023)
//            if (_mapper == null)
//            {
//                var mappingConfig = new MapperConfiguration(mc =>
//                {
//                    mc.AddProfile(new MapperProfile());
//                });
//                IMapper mapper = mappingConfig.CreateMapper();
//                _mapper = mapper;
//            }
//        }

//        //kan create navigation testes? mtp at det er bilder i filsystem
//        [Fact]
//        public async Task GivenSignedInUser_WhenCreateNavigatoin_ThenCreate()
//        {
//            //fix test  gettrackuserbyquiz
//            //ARRANGE
//            var _db = new OrienteeringContext(dbContextOptions, null);
//            if (!_db.Database.IsInMemory()) { _db.Database.Migrate(); }
//            var userId=Guid.NewGuid();

//            var identityService = new Mock<IIdentityService>();
//            identityService.Setup(i=>i.GetCurrentUserId()).Returns(userId);

//            var mediator = new Mock<IMediator>();
//            //identityService.Setup(i => i.GetCurrentUserId()).Returns(userId);

//            //var request = new DeleteTrack.Request(track.Id);
//            //var handler = new DeleteTrack.Handler(_db, identityService.Object, mediator.Object);

//            //ACT

//            //var response = handler.Handle(request, CancellationToken.None).GetAwaiter().GetResult();


//            //ASSERT
//        }


//        [Fact]
//        public async Task GivenSignedInUser_WhenRetriveNavigation_Then1()
//        {
//            //ARRANGE
//            var _db = new OrienteeringContext(dbContextOptions, null);
//            if (!_db.Database.IsInMemory()) { _db.Database.Migrate(); }

//            var identityService = new Mock<IIdentityService>();
//            //identityService.Setup(i => i.GetCurrentUserId()).Returns(userId);

//            //var request = new DeleteTrack.Request(track.Id);
//            //var handler = new DeleteTrack.Handler(_db, identityService.Object, mediator.Object);

//            //ACT

//            //var response = handler.Handle(request, CancellationToken.None).GetAwaiter().GetResult();


//            //ASSERT
//        }



//        //mal
//        [Fact]
//        public async Task Given_When_Then1()
//        {
//            //ARRANGE
//            var _db = new OrienteeringContext(dbContextOptions, null);
//            if (!_db.Database.IsInMemory()) { _db.Database.Migrate(); }

//            var identityService = new Mock<IIdentityService>();
//            //identityService.Setup(i => i.GetCurrentUserId()).Returns(userId);

//            //var request = new DeleteTrack.Request(track.Id);
//            //var handler = new DeleteTrack.Handler(_db, identityService.Object, mediator.Object);

//            //ACT

//            //var response = handler.Handle(request, CancellationToken.None).GetAwaiter().GetResult();


//            //ASSERT
//        }
//    }
//}
