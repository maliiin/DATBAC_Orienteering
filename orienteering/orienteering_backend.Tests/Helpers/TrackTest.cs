using AutoMapper;
using Microsoft.EntityFrameworkCore;
using Moq;
using Newtonsoft.Json;
using orienteering_backend.Core.Domain.Track.Dto;
using orienteering_backend.Core.Domain.Track.Pipelines;
using orienteering_backend.Core.Domain.Track;
using orienteering_backend.Infrastructure.Data;
using orienteering_backend.Infrastructure.Automapper;
using orienteering_backend.Core.Domain.Authentication.Services;
using System.Security.Authentication;
using MediatR;
//fix mangler
//getsingletrackunauthorized
//gettrackuserbyquiz
//setstartcheckpoint

namespace orienteering_backend.Tests.Helpers
{
    public class TrackTest
    {
        public readonly DbContextOptions<OrienteeringContext> dbContextOptions;

        private static IMapper _mapper;

        public TrackTest()
        {
            dbContextOptions = new DbContextOptionsBuilder<OrienteeringContext>()
                .UseInMemoryDatabase(databaseName: "orienteeringTest")
                .Options;

            // "Mocker" automapper Fix bruker mock nå heller
            //Kilder: https://www.thecodebuzz.com/unit-test-mock-automapper-asp-net-core-imapper/ (06.03.2023)
            if (_mapper == null)
            {
                var mappingConfig = new MapperConfiguration(mc =>
                {
                    mc.AddProfile(new MapperProfile());
                });
                IMapper mapper = mappingConfig.CreateMapper();
                _mapper = mapper;
            }
        }

        [Fact]
        public async Task TestGetTrackUser()
        {
            //arrange
            var _db = new OrienteeringContext(dbContextOptions, null);
            if (!_db.Database.IsInMemory()) { _db.Database.Migrate(); }

            //add track to db
            var track = new Track();
            track.Name = "name";
            track.UserId = Guid.NewGuid();
            //track.UserId = Guid.NewGuid();
            await _db.Tracks.AddAsync(track);
            await _db.SaveChangesAsync();

            var trackDb = await _db.Tracks.Where(t => t.Name == "name").FirstOrDefaultAsync();
            var expected = new TrackUserIdDto();
            expected.UserId = track.UserId;
            expected.TrackId = trackDb.Id;

            //var mapper = _mapper;
            var mapper = new Mock<IMapper>();
            mapper.Setup(x => x.Map<Track, TrackUserIdDto>(track)).Returns(expected);

            var request = new GetTrackUser.Request(trackDb.Id);
            var handler = new GetTrackUser.Handler(_db, mapper.Object);

            //act
            var response = handler.Handle(request, CancellationToken.None).GetAwaiter().GetResult();
            Assert.Equal(JsonConvert.SerializeObject(expected), JsonConvert.SerializeObject(response));
        }

        [Fact]
        public async Task GivenUser_WhenCreateTrack_ThenCreate()
        {
            //ARRANGE
            var db = new OrienteeringContext(dbContextOptions, null);
            if (!db.Database.IsInMemory()) { db.Database.Migrate(); }

            var testUserId = Guid.NewGuid();
            var createTrackDto = new CreateTrackDto("trackName");

            var realTrack = new Track();
            realTrack.Name = "trackName";
            realTrack.UserId = testUserId;

            var identityService = new Mock<IIdentityService>();
            identityService.Setup(i => i.GetCurrentUserId()).Returns(testUserId);

            var mapper = new Mock<IMapper>();
            mapper.Setup(x => x.Map<CreateTrackDto, Track>(createTrackDto)).Returns(realTrack);

            var request = new CreateTrack.Request(createTrackDto);
            var handler = new CreateTrack.Handler(db, mapper.Object, identityService.Object);

            //ACT
            var result = handler.Handle(request, CancellationToken.None).GetAwaiter().GetResult();

            //ASSERT
            Assert.IsType<Guid>(result);

        }

        [Fact]
        public async Task GivenNoUser_WhenCreateTrack_ThenFail()
        {
            //ARRANGE
            var db = new OrienteeringContext(dbContextOptions, null);
            if (!db.Database.IsInMemory()) { db.Database.Migrate(); }

            var userId = Guid.NewGuid();
            var createTrackDto = new CreateTrackDto("trackName");

            var realTrack = new Track();
            realTrack.Name = "trackName";
            realTrack.UserId = userId;

            var identityService = new Mock<IIdentityService>();
            identityService.Setup(i => i.GetCurrentUserId()).Returns<Guid?>(null);

            var mapper = new Mock<IMapper>();
            mapper.Setup(x => x.Map<CreateTrackDto, Track>(createTrackDto)).Returns(realTrack);

            var request = new CreateTrack.Request(createTrackDto);
            var handler = new CreateTrack.Handler(db, mapper.Object, identityService.Object);

            //ACT AND ASSERT
            Assert.Throws<AuthenticationException>(() => handler.Handle(request, CancellationToken.None).GetAwaiter().GetResult());
        }

        [Fact]
        public async Task GivenUser_WhenAskForTrack_ThenReturnTrack()
        {
            //ARRANGE
            var _db = new OrienteeringContext(dbContextOptions, null);
            if (!_db.Database.IsInMemory()) { _db.Database.Migrate(); }
            var userId = Guid.NewGuid();

            var track = new Track();
            track.UserId = userId;
            track.Name = "Test";
            await _db.Tracks.AddAsync(track);
            await _db.SaveChangesAsync();

            var expectedTrackDto = new TrackDto();
            expectedTrackDto.TrackName = "Test";

            var mapper = new Mock<IMapper>();
            mapper.Setup(x => x.Map<Track, TrackDto>(track)).Returns(expectedTrackDto);

            var identityService = new Mock<IIdentityService>();
            identityService.Setup(i => i.GetCurrentUserId()).Returns(userId);

            var request = new GetSingleTrack.Request(track.Id);
            var handler = new GetSingleTrack.Handler(_db, mapper.Object, identityService.Object);

            //ACT
            var returnedTrackDto = handler.Handle(request, CancellationToken.None).GetAwaiter().GetResult();

            //ASSERT
            Assert.Equal(JsonConvert.SerializeObject(expectedTrackDto), JsonConvert.SerializeObject(returnedTrackDto));
        }

        [Fact]
        public async Task GivenCorrectUser_WhenDeleteTrack_ThenDeleteTrack()
        {
            //ARRANGE
            var _db = new OrienteeringContext(dbContextOptions, null);
            if (!_db.Database.IsInMemory()) { _db.Database.Migrate(); }

            //create track
            var userId = Guid.NewGuid();
            var track = new Track();
            track.UserId = userId;
            track.Name = "Test";
            await _db.Tracks.AddAsync(track);
            await _db.SaveChangesAsync();

            var identityService = new Mock<IIdentityService>();
            identityService.Setup(i => i.GetCurrentUserId()).Returns(userId);
            var mediator = new Mock<IMediator>();
            //mediator.Setup(m=>m.)

            var request = new DeleteTrack.Request(track.Id);
            var handler = new DeleteTrack.Handler(_db, identityService.Object, mediator.Object);

            //ACT
            var response = handler.Handle(request, CancellationToken.None).GetAwaiter().GetResult();

            //ASSERT
            Assert.True(response);
            var dbTrack =await _db.Tracks.Where(t=>t.UserId== userId).FirstOrDefaultAsync();
            Assert.Null(dbTrack);
        }

        [Fact]
        public async Task GivenCorrectUser_WhenGetTracks_ThenReturnTracks()
        {
            //test GetTracks
            //ARRANGE
            var userId=Guid.NewGuid();

            var identityService = new Mock<IIdentityService>();
            identityService.Setup(i => i.GetCurrentUserId()).Returns(userId);

            var _db = new OrienteeringContext(dbContextOptions, null);
            if (!_db.Database.IsInMemory()) { _db.Database.Migrate(); }

            //add tracks to db
            var track1 = new Track();
            track1.UserId = userId;
            track1.Name = "Test1";
            var track2 = new Track();
            track2.UserId = userId;
            track2.Name = "Test2";
            await _db.Tracks.AddAsync(track1);
            await _db.Tracks.AddAsync(track2);
            await _db.SaveChangesAsync();

            //expected output
            var track1Dto = _mapper.Map<Track, TrackDto>(track1);
            var track2Dto = _mapper.Map<Track, TrackDto>(track2);
            var expectedList = new List<TrackDto> { track1Dto, track2Dto };

            var request = new GetTracks.Request();
            var handler = new GetTracks.Handler(_db,_mapper, identityService.Object);

            //ACT

            var response = handler.Handle(request, CancellationToken.None).GetAwaiter().GetResult();


            //ASSERT
            Assert.Equal(JsonConvert.SerializeObject(expectedList), JsonConvert.SerializeObject(response));
        }

        //[Fact]
        //public async Task Given_When_Then1()
        //{
        //    //fix test  gettrackuserbyquiz
        //    //ARRANGE
        //    var _db = new OrienteeringContext(dbContextOptions, null);
        //    if (!_db.Database.IsInMemory()) { _db.Database.Migrate(); }


        //    var identityService = new Mock<IIdentityService>();
        //    //identityService.Setup(i => i.GetCurrentUserId()).Returns(userId);

        //    //var request = new DeleteTrack.Request(track.Id);
        //    //var handler = new DeleteTrack.Handler(_db, identityService.Object, mediator.Object);

        //    //ACT

        //    //var response = handler.Handle(request, CancellationToken.None).GetAwaiter().GetResult();


        //    //ASSERT
        //}


        //[Fact]
        //public async Task Given_When_Then2()
        //{
        //    //test check start checkpoint
        //    //ARRANGE

        //    var identityService = new Mock<IIdentityService>();
        //    //identityService.Setup(i => i.GetCurrentUserId()).Returns(userId);

        //    //var request = new DeleteTrack.Request(track.Id);
        //    //var handler = new DeleteTrack.Handler(_db, identityService.Object, mediator.Object);

        //    //ACT

        //    //var response = handler.Handle(request, CancellationToken.None).GetAwaiter().GetResult();


        //    //ASSERT
        //}

        [Fact]
        public async Task GivenCorrectUser_WhenUpdateTrackTitle_ThenUpdate()
        {
            //test update track title
            //ARRANGE
            var _db = new OrienteeringContext(dbContextOptions, null);
            if (!_db.Database.IsInMemory()) { _db.Database.Migrate(); }
            var userId = Guid.NewGuid();

            var identityService = new Mock<IIdentityService>();
            identityService.Setup(i => i.GetCurrentUserId()).Returns(userId);

            //create track
            var track = new Track();
            track.UserId = userId;
            track.Name = "Test1";
            await _db.Tracks.AddAsync(track);
            await _db.SaveChangesAsync();

            //expected output
            var newTitle = "this is the new title";
            track.Name = newTitle;

            var request = new UpdateTrackTitle.Request(track.Id, newTitle );
            var handler = new UpdateTrackTitle.Handler(_db, identityService.Object);

            //ACT

            var response = handler.Handle(request, CancellationToken.None).GetAwaiter().GetResult();
            var changedTrack=await _db.Tracks.Where(t => t.UserId == track.UserId).FirstOrDefaultAsync();

            //ASSERT
            Assert.True(response);
            Assert.Equal(JsonConvert.SerializeObject(track), JsonConvert.SerializeObject(changedTrack));
        }


        //fix slett denne, dette er mal

        //[Fact]
        //public async Task Given_When_Then()
        //{
        //    //ARRANGE

        //    var identityService = new Mock<IIdentityService>();
        //    //identityService.Setup(i => i.GetCurrentUserId()).Returns(userId);

        //    //var request = new DeleteTrack.Request(track.Id);
        //    //var handler = new DeleteTrack.Handler(_db, identityService.Object, mediator.Object);

        //    //ACT

        //    //var response = handler.Handle(request, CancellationToken.None).GetAwaiter().GetResult();


        //    //ASSERT
        //}

    }
}
