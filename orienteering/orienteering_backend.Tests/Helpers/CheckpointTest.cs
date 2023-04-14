using AutoMapper;
using Microsoft.EntityFrameworkCore;
using Moq;
using Newtonsoft.Json;
using orienteering_backend.Core.Domain.Track.Dto;
using orienteering_backend.Core.Domain.Track.Pipelines;
using orienteering_backend.Core.Domain.Track;
using orienteering_backend.Infrastructure.Automapper;
using orienteering_backend.Infrastructure.Data;
using MediatR;
using orienteering_backend.Core.Domain.Authentication.Services;
using orienteering_backend.Core.Domain.Checkpoint.Dto;
using orienteering_backend.Core.Domain.Checkpoint;
using orienteering_backend.Core.Domain.Checkpoint.Pipelines;
using System.Security.Authentication;

//fix-legg inn lisens her!
//manglende tester
//getQrCodes

namespace orienteering_backend.Tests.Helpers
{
    public class CheckpointTest
    {
        public readonly DbContextOptions<OrienteeringContext> dbContextOptions;

        private static IMapper _mapper;
        public CheckpointTest()
        {
            dbContextOptions = new DbContextOptionsBuilder<OrienteeringContext>()
               .UseInMemoryDatabase(databaseName: "orienteeringTest")
               .Options;

            // "Mocker" automapper Fix bruke denne alltid istedenfor moq på automapper
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
        public async Task GivenCorrectUser_WhenCreateCheckpoint_ThenCreate()
        {
            //ARRANGE
            var _db = new OrienteeringContext(dbContextOptions, null);
            if (!_db.Database.IsInMemory()) { _db.Database.Migrate(); }

            var userId = Guid.NewGuid();

            Track track = new();
            track.UserId = userId;
            track.Name = "trackname";
            await _db.Tracks.AddAsync(track);

            var trackDto=_mapper.Map<TrackDto>(track);

            Checkpoint expectedCheckpoint = new("test1", 0, track.Id);
            CheckpointDto checkpointDto = new("test1", track.Id, 0);

            var _identityService = new Mock<IIdentityService>();
            _identityService.Setup(i => i.GetCurrentUserId()).Returns(userId);

            var _mediator = new Mock<IMediator>();
            _mediator.Setup(m => m.Send(It.IsAny<GetSingleTrackUnauthorized.Request>(), It.IsAny<CancellationToken>())).ReturnsAsync(trackDto);

            var request = new CreateCheckpoint.Request(checkpointDto);
            var handler = new CreateCheckpoint.Handler(_db, _mediator.Object, _identityService.Object);

            //ACT
            var response = handler.Handle(request, CancellationToken.None).GetAwaiter().GetResult();
            //var allCheckpoint = await _db.Checkpoints.ToListAsync();
            //var h = trackDto.TrackId;
            //var createdCheckpoint=await _db.Checkpoints.Where(c => c.TrackId == trackDto.TrackId).FirstOrDefaultAsync();
            //createdCheckpoint.Id = new Guid();

            //ASSERT
            Assert.IsType<Guid>(response);

            //Assert.Equal(JsonConvert.SerializeObject(expectedCheckpoint), JsonConvert.SerializeObject(createdCheckpoint));


            //fix denne testen. created checkpoint inneholder quiz id og sånn som ikke finnes ellers.
            //så vanskelig å sammenligne objektene direkte.



        }

        [Fact]
        public async Task GivenCorretUser_WhenDeleteCheckpoint_ThenDeleteCheckpoint()
        {
            //ARRANGE
            var _db = new OrienteeringContext(dbContextOptions, null);
            if (!_db.Database.IsInMemory()) { _db.Database.Migrate(); }

            var userId = Guid.NewGuid();

            //create track
            var track = new Track();
            track.Name = "trackname";
            track.UserId = userId;
            await _db.Tracks.AddAsync(track);
            await _db.SaveChangesAsync();

            //var Track = await _db.Tracks.FirstOrDefaultAsync();
            //var trackId = Track.Id;

            //create checkpoint
            var checkpoint = new Checkpoint("title", 1, track.Id);
            track.AddedCheckpoint();
            await _db.Checkpoints.AddAsync(checkpoint);
            await _db.SaveChangesAsync();

            //trackUserDto
            var trackUserDto = _mapper.Map<TrackUserIdDto>(track);

            //mock
            var _identityService = new Mock<IIdentityService>();
            _identityService.Setup(i => i.GetCurrentUserId()).Returns(userId);

            var _mediator = new Mock<IMediator>();
            _mediator.Setup(m => m.Send(It.IsAny<GetTrackUser.Request>(), It.IsAny<CancellationToken>())).ReturnsAsync(trackUserDto);

            var request = new DeleteCheckpoint.Request(checkpoint.Id);
            var handler = new DeleteCheckpoint.Handler(_db, _identityService.Object, _mediator.Object);

            //ACT
            var response = handler.Handle(request, CancellationToken.None).GetAwaiter().GetResult();

            var isDeleted = await _db.Checkpoints.Where(c => c.TrackId == track.Id).FirstOrDefaultAsync();

            //ASSERT
            Assert.True(response);
            Assert.Null(isDeleted);

        }


        [Fact]
        public async Task GivenNoUser_WhenDeleteCheckpoint_ThenException()
        {
            //ARRANGE
            var _db = new OrienteeringContext(dbContextOptions, null);
            if (!_db.Database.IsInMemory()) { _db.Database.Migrate(); }

            var checkpointId = Guid.NewGuid();

            //mock
            var _identityService = new Mock<IIdentityService>();
            _identityService.Setup(i => i.GetCurrentUserId()).Returns<Guid?>(null);

            var _mediator = new Mock<IMediator>();

            var request = new DeleteCheckpoint.Request(checkpointId);
            var handler = new DeleteCheckpoint.Handler(_db, _identityService.Object, _mediator.Object);

            //ACT and assert
            Assert.Throws<AuthenticationException>(() => handler.Handle(request, CancellationToken.None).GetAwaiter().GetResult());

        }

        [Fact]
        public async Task GivenCorrectUser_WhenGetCheckpointForTrack_ThenSuccess()
        {
            //ARRANGE
            var _db = new OrienteeringContext(dbContextOptions, null);
            if (!_db.Database.IsInMemory()) { _db.Database.Migrate(); }

            var userId = Guid.NewGuid();

            //create track
            var track = new Track();
            track.Name = "Test";
            track.UserId = userId;
            await _db.Tracks.AddAsync(track);
            await _db.SaveChangesAsync();

            //var trackDb = await _db.Tracks..FirstOrDefaultAsync();
            TrackUserIdDto trackUserIdDto = _mapper.Map<TrackUserIdDto>(track);

            //create checkpoints
            var checkpoint1 = new Checkpoint("test1", 0, track.Id);
            var checkpoint2 = new Checkpoint("test2", 0, track.Id);

            await _db.Checkpoints.AddAsync(checkpoint1);
            await _db.Checkpoints.AddAsync(checkpoint2);
            await _db.SaveChangesAsync();

            var checkpoint1Dto = _mapper.Map<Checkpoint, CheckpointDto>(checkpoint1);
            var checkpoint2Dto = _mapper.Map<Checkpoint, CheckpointDto>(checkpoint2);

            //mock
            var _identityService = new Mock<IIdentityService>();
            _identityService.Setup(i => i.GetCurrentUserId()).Returns(userId);

            var _mediator = new Mock<IMediator>();
            _mediator.Setup(m => m.Send(It.IsAny<GetTrackUser.Request>(), It.IsAny<CancellationToken>())).ReturnsAsync(trackUserIdDto);

            var request = new GetCheckpointsForTrack.Request(track.Id);
            var handler = new GetCheckpointsForTrack.Handler(_db, _mapper, _mediator.Object, _identityService.Object);

            //ACT
            var response = handler.Handle(request, CancellationToken.None).GetAwaiter().GetResult();

            //ASSERT
            Assert.Equal(JsonConvert.SerializeObject(response[0]), JsonConvert.SerializeObject(checkpoint1Dto));
            Assert.Equal(JsonConvert.SerializeObject(response[1]), JsonConvert.SerializeObject(checkpoint2Dto));

        }


        [Fact]
        public async Task GivenLastCheckpoint_WhenGetNextCheckpoint_ThenSuccess()
        {
            //ARRANGE
            var _db = new OrienteeringContext(dbContextOptions, null);
            if (!_db.Database.IsInMemory()) { _db.Database.Migrate(); }

            var userId = Guid.NewGuid();

            //create track
            var track = new Track();
            track.Name = "testName";
            track.UserId = userId;
            await _db.Tracks.AddAsync(track);
            await _db.SaveChangesAsync();

            var trackDb = await _db.Tracks.FirstOrDefaultAsync();
            var trackDto = _mapper.Map<TrackDto>(trackDb);

            //create checkpoints
            var checkpoint1 = new Checkpoint("test1", 0, trackDb.Id);
            checkpoint1.Order = 1;
            var checkpoint2 = new Checkpoint("test2", 0, trackDb.Id);
            checkpoint2.Order = 2;
            var checkpoint3 = new Checkpoint("test3", 0, trackDb.Id);
            checkpoint3.Order = 3;
            //add to db
            await _db.Checkpoints.AddAsync(checkpoint1);
            track.AddedCheckpoint();
            await _db.Checkpoints.AddAsync(checkpoint2);
            track.AddedCheckpoint();
            await _db.Checkpoints.AddAsync(checkpoint3);
            track.AddedCheckpoint();
            await _db.SaveChangesAsync();

            //mock
            var _mediator = new Mock<IMediator>();
            _mediator.Setup(m => m.Send(It.IsAny<GetSingleTrackUnauthorized.Request>(), It.IsAny<CancellationToken>())).ReturnsAsync(trackDto);

            var request = new GetNextCheckpoint.Request(checkpoint3.Id);
            var handler = new GetNextCheckpoint.Handler(_db, _mediator.Object);

            //ACT
            var response = handler.Handle(request, CancellationToken.None).GetAwaiter().GetResult();

            //ASSERT
            Assert.Equal(checkpoint1.Id, response);

        }


        [Fact]
        public async Task GivenMiddleCheckpoint_WhenGetNextCheckpoint_ThenSuccess()
        {
            //ARRANGE
            var _db = new OrienteeringContext(dbContextOptions, null);
            if (!_db.Database.IsInMemory()) { _db.Database.Migrate(); }

            var userId = Guid.NewGuid();

            //create track
            var track = new Track();
            track.Name = "testNameTrack";
            track.UserId = userId;
            await _db.Tracks.AddAsync(track);
            await _db.SaveChangesAsync();

            //create checkpoints
            var checkpoint1 = new Checkpoint("test1", 0, track.Id);
            checkpoint1.Order = 1;
            var checkpoint2 = new Checkpoint("test2", 0, track.Id);
            checkpoint2.Order = 2;
            var checkpoint3 = new Checkpoint("test3", 0, track.Id);
            checkpoint3.Order = 3;
            //add to db
            await _db.Checkpoints.AddAsync(checkpoint1);
            track.AddedCheckpoint();
            await _db.Checkpoints.AddAsync(checkpoint2);
            track.AddedCheckpoint();
            await _db.Checkpoints.AddAsync(checkpoint3);
            track.AddedCheckpoint();
            await _db.SaveChangesAsync();

            var trackDto = _mapper.Map<TrackDto>(track);

            //mock
            var _mediator = new Mock<IMediator>();
            _mediator.Setup(m => m.Send(It.IsAny<GetSingleTrackUnauthorized.Request>(), It.IsAny<CancellationToken>())).ReturnsAsync(trackDto);

            var request = new GetNextCheckpoint.Request(checkpoint2.Id);
            var handler = new GetNextCheckpoint.Handler(_db, _mediator.Object);

            //ACT
            var response = handler.Handle(request, CancellationToken.None).GetAwaiter().GetResult();

            //ASSERT
            Assert.Equal(checkpoint3.Id, response);

        }

        [Fact]
        public async Task Given_WhenGetQuizIdOfCheckpoint_ThenSuccess()
        {
            //ARRANGE
            var _db = new OrienteeringContext(dbContextOptions, null);
            if (!_db.Database.IsInMemory()) { _db.Database.Migrate(); }

            //create checkpoint
            var quizId = Guid.NewGuid();
            var checkpoint = new Checkpoint("title", 0, Guid.NewGuid());
            checkpoint.QuizId = quizId;
            await _db.Checkpoints.AddAsync(checkpoint);
            await _db.SaveChangesAsync();

            var checkpointDb = await _db.Checkpoints.Where(c => c.Id == checkpoint.Id).FirstOrDefaultAsync();

            var request = new GetQuizIdOfCheckpoint.Request(checkpointDb.Id);
            var handler = new GetQuizIdOfCheckpoint.Handler(_db);

            //ACT
            var response = handler.Handle(request, CancellationToken.None).GetAwaiter().GetResult();

            //ASSERT
            Assert.Equal(quizId, response);

        }

        [Fact]
        public async Task GivenCorrectUser_WhenGetSingleCheckpoint_ThenSuccess()
        {
            //ARRANGE
            var _db = new OrienteeringContext(dbContextOptions, null);
            if (!_db.Database.IsInMemory()) { _db.Database.Migrate(); }

            var userId = Guid.NewGuid();

            //create track
            var track = new Track();
            track.UserId = userId;
            track.Name = "trackname";
            await _db.Tracks.AddAsync(track);
            await _db.SaveChangesAsync();

            //create checkpoint
            var checkpoint = new Checkpoint("test1", 0, track.Id);
            await _db.Checkpoints.AddAsync(checkpoint);
            track.AddedCheckpoint();
            await _db.SaveChangesAsync();

            //dto
            TrackUserIdDto trackDto = _mapper.Map<TrackUserIdDto>(track);
            CheckpointDto checkpointDto = _mapper.Map<CheckpointDto>(checkpoint);


            var _identityService = new Mock<IIdentityService>();
            _identityService.Setup(i => i.GetCurrentUserId()).Returns(userId);


            var _mediator = new Mock<IMediator>();
            _mediator.Setup(m => m.Send(It.IsAny<GetTrackUser.Request>(), It.IsAny<CancellationToken>())).ReturnsAsync(trackDto);

            var request = new GetSingleCheckpoint.Request(checkpoint.Id);
            var handler = new GetSingleCheckpoint.Handler(_db, _mapper, _identityService.Object, _mediator.Object);

            //ACT
            var response = handler.Handle(request, CancellationToken.None).GetAwaiter().GetResult();

            //ASSERT
            Assert.Equal(JsonConvert.SerializeObject(checkpointDto), JsonConvert.SerializeObject(response));
        }


        [Fact]
        public async Task GivenTrackAndQuiz_WhenGetTrackIdForQuiz_ThenGetId()
        {
            //ARRANGE
            var _db = new OrienteeringContext(dbContextOptions, null);
            if (!_db.Database.IsInMemory()) { _db.Database.Migrate(); }


            var trackId = Guid.NewGuid();
            var quizId = Guid.NewGuid();
            var checkpoint = new Checkpoint("title", 0, trackId);
            checkpoint.QuizId = quizId;
            await _db.Checkpoints.AddAsync(checkpoint);
            await _db.SaveChangesAsync();

            var request = new GetTrackIdForQuiz.Request(quizId);
            var handler = new GetTrackIdForQuiz.Handler(_db);

            //ACT
            var response = handler.Handle(request, CancellationToken.None).GetAwaiter().GetResult();

            //ASSERT
            Assert.Equal(trackId, response);
        }

        [Fact]
        public async Task GivenCorrectUser_WhenUpdateCheckpointTitle_ThenUpdate()
        {
            //ARRANGE
            var _db = new OrienteeringContext(dbContextOptions, null);
            if (!_db.Database.IsInMemory()) { _db.Database.Migrate(); }

            var userId = Guid.NewGuid();

            //create track
            var track = new Track();
            track.UserId = userId;
            track.Name = "trackname";
            await _db.Tracks.AddAsync(track);
            await _db.SaveChangesAsync();

            //create checkpoint
            var checkpoint = new Checkpoint("test1", 0, track.Id);
            await _db.Checkpoints.AddAsync(checkpoint);
            track.AddedCheckpoint();
            await _db.SaveChangesAsync();

            //dto
            TrackUserIdDto trackDto = _mapper.Map<TrackUserIdDto>(track);
            CheckpointDto checkpointDto = _mapper.Map<CheckpointDto>(checkpoint);

            var newTitle = "newTitle";
            var expectedCheckpoint = checkpoint;
            expectedCheckpoint.Title = newTitle;

            var _identityService = new Mock<IIdentityService>();
            _identityService.Setup(i => i.GetCurrentUserId()).Returns(userId);


            var _mediator = new Mock<IMediator>();
            _mediator.Setup(m => m.Send(It.IsAny<GetTrackUser.Request>(), It.IsAny<CancellationToken>())).ReturnsAsync(trackDto);

            var request = new UpdateCheckpointTitle.Request(newTitle, checkpoint.Id);
            var handler = new UpdateCheckpointTitle.Handler(_db, _identityService.Object, _mediator.Object);

            //ACT
            var response = handler.Handle(request, CancellationToken.None).GetAwaiter().GetResult();

            //ASSERT
            Assert.Equal(JsonConvert.SerializeObject(expectedCheckpoint), JsonConvert.SerializeObject(response));
        }

        //fix slett denne
        [Fact]
        public async Task MAL()
        {
            //ARRANGE
            var _db = new OrienteeringContext(dbContextOptions, null);
            if (!_db.Database.IsInMemory()) { _db.Database.Migrate(); }

            var userId = Guid.NewGuid();

            var _identityService = new Mock<IIdentityService>();
            _identityService.Setup(i => i.GetCurrentUserId()).Returns<Guid?>(null);


            var _mediator = new Mock<IMediator>();
            //_mediator.Setup(m => m.Send(It.IsAny<GetSingleTrack.Request>(), It.IsAny<CancellationToken>())).ReturnsAsync(trackDto);

            //var request = new CreateCheckpoint.Request(checkpointDto, userId);
            //var handler = new CreateCheckpoint.Handler(_db, _mediator.Object);

            //ACT
            //var response = handler.Handle(request, CancellationToken.None).GetAwaiter().GetResult();

        }
    }
}
