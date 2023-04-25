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
using Xunit;
// Licence Xunit (Apache 2.0): https://github.com/xunit/xunit/blob/main/LICENSE
// License MediatR (Apache 2.0): https://github.com/jbogard/MediatR/blob/master/LICENSE

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

            // "Mocker" automapper 
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
            var _db = new OrienteeringContext(dbContextOptions);
            if (!_db.Database.IsInMemory()) { _db.Database.Migrate(); }

            var userId = Guid.NewGuid();

            Track track = new();
            track.UserId = userId;
            track.Name = "trackname";
            await _db.Tracks.AddAsync(track);

            var trackDto = _mapper.Map<TrackDto>(track);

            Checkpoint expectedCheckpoint = new("test1", 0, track.Id);
            expectedCheckpoint.CheckpointDescription= "test1";
            CheckpointDto checkpointDto = new("test1CreateCheckpoint", track.Id, 0);
            checkpointDto.CheckpointDescription = "test1";

            var _identityService = new Mock<IIdentityService>();
            _identityService.Setup(i => i.GetCurrentUserId()).Returns(userId);

            var _mediator = new Mock<IMediator>();
            _mediator.Setup(m => m.Send(It.IsAny<GetSingleTrackUnauthorized.Request>(), It.IsAny<CancellationToken>())).ReturnsAsync(trackDto);

            var request = new CreateCheckpoint.Request(checkpointDto);
            var handler = new CreateCheckpoint.Handler(_db, _mediator.Object, _identityService.Object, _mapper);

            //ACT
            var response = handler.Handle(request, CancellationToken.None).GetAwaiter().GetResult();
            var createdCheckpoint = await _db.Checkpoints.Where(c => c.Title == checkpointDto.Title).FirstOrDefaultAsync();

            //ASSERT
            Assert.IsType<Guid>(response);
            Assert.NotNull(createdCheckpoint);
            Assert.Equal(expectedCheckpoint.TrackId, createdCheckpoint.TrackId);
        }

        [Fact]
        public async Task GivenCorretUser_WhenDeleteCheckpoint_ThenDeleteCheckpoint()
        {
            //ARRANGE
            var _db = new OrienteeringContext(dbContextOptions);
            if (!_db.Database.IsInMemory()) { _db.Database.Migrate(); }

            var userId = Guid.NewGuid();

            //create track
            var track = new Track();
            track.Name = "trackname";
            track.UserId = userId;
            await _db.Tracks.AddAsync(track);
            await _db.SaveChangesAsync();

            //create checkpoint
            var checkpoint = new Checkpoint("title", 1, track.Id);
            checkpoint.CheckpointDescription = "Test";
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
        public void GivenNoUser_WhenDeleteCheckpoint_ThenException()
        {
            //ARRANGE
            var _db = new OrienteeringContext(dbContextOptions);
            if (!_db.Database.IsInMemory()) { _db.Database.Migrate(); }

            var checkpointId = Guid.NewGuid();

            //mock
            var _identityService = new Mock<IIdentityService>();
            _identityService.Setup(i => i.GetCurrentUserId()).Returns<Guid?>(null);

            var _mediator = new Mock<IMediator>();

            var request = new DeleteCheckpoint.Request(checkpointId);
            var handler = new DeleteCheckpoint.Handler(_db, _identityService.Object, _mediator.Object);

            //ACT and ASSERT
            Assert.Throws<AuthenticationException>(() => handler.Handle(request, CancellationToken.None).GetAwaiter().GetResult());
        }

        [Fact]
        public async Task GivenCorrectUser_WhenGetCheckpointForTrack_ThenGetCheckpoints()
        {
            //ARRANGE
            var _db = new OrienteeringContext(dbContextOptions);
            if (!_db.Database.IsInMemory()) { _db.Database.Migrate(); }

            var userId = Guid.NewGuid();

            //create track
            var track = new Track();
            track.Name = "Test";
            track.UserId = userId;
            await _db.Tracks.AddAsync(track);
            await _db.SaveChangesAsync();

            TrackUserIdDto trackUserIdDto = _mapper.Map<TrackUserIdDto>(track);

            //create checkpoints
            var checkpoint1 = new Checkpoint("test1", 0, track.Id);
            checkpoint1.CheckpointDescription = "test1";
            var checkpoint2 = new Checkpoint("test2", 0, track.Id);
            checkpoint2.CheckpointDescription = "test2";

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
            var _db = new OrienteeringContext(dbContextOptions);
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
            checkpoint1.CheckpointDescription = "Test1";
            var checkpoint2 = new Checkpoint("test2", 0, trackDb.Id);
            checkpoint2.Order = 2;
            checkpoint2.CheckpointDescription = "Test2";
            var checkpoint3 = new Checkpoint("test3", 0, trackDb.Id);
            checkpoint3.Order = 3;
            checkpoint3.CheckpointDescription = "Test3";

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
            var _db = new OrienteeringContext(dbContextOptions);
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
            checkpoint1.CheckpointDescription = "Test1";
            var checkpoint2 = new Checkpoint("test2", 0, track.Id);
            checkpoint2.Order = 2;
            checkpoint2.CheckpointDescription = "Test2";
            var checkpoint3 = new Checkpoint("test3", 0, track.Id);
            checkpoint3.Order = 3;
            checkpoint3.CheckpointDescription = "Test3";
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
        public async Task GivenQuizId_WhenGetQuizIdOfCheckpoint_ThenSuccess()
        {
            //ARRANGE
            var _db = new OrienteeringContext(dbContextOptions);
            if (!_db.Database.IsInMemory()) { _db.Database.Migrate(); }

            //create checkpoint
            var quizId = Guid.NewGuid();
            var checkpoint = new Checkpoint("title", 0, Guid.NewGuid());
            checkpoint.CheckpointDescription = "Test";
            checkpoint.CheckpointDescription = "test";
            checkpoint.QuizId = quizId;
            await _db.Checkpoints.AddAsync(checkpoint);
            await _db.SaveChangesAsync();

            //var checkpointDb = await _db.Checkpoints.Where(c => c.Id == checkpoint.Id).FirstOrDefaultAsync();

            var request = new GetQuizIdOfCheckpoint.Request(checkpoint.Id);
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
            var _db = new OrienteeringContext(dbContextOptions);
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
            checkpoint.CheckpointDescription = "test";
            checkpoint.CheckpointDescription = "TestDes";

            await _db.Checkpoints.AddAsync(checkpoint);
            track.AddedCheckpoint();
            await _db.SaveChangesAsync();

            //dto
            TrackUserIdDto trackDto = _mapper.Map<TrackUserIdDto>(track);
            CheckpointDto checkpointDto = _mapper.Map<CheckpointDto>(checkpoint);

            //mock
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
            var _db = new OrienteeringContext(dbContextOptions);
            if (!_db.Database.IsInMemory()) { _db.Database.Migrate(); }

            var trackId = Guid.NewGuid();
            var quizId = Guid.NewGuid();
            var checkpoint = new Checkpoint("title", 0, trackId);
            checkpoint.QuizId = quizId;
            checkpoint.CheckpointDescription = "test";

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
            var _db = new OrienteeringContext(dbContextOptions);
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
            checkpoint.CheckpointDescription = "test";

            await _db.Checkpoints.AddAsync(checkpoint);
            track.AddedCheckpoint();
            await _db.SaveChangesAsync();

            //dto
            TrackUserIdDto trackDto = _mapper.Map<TrackUserIdDto>(track);
            CheckpointDto checkpointDto = _mapper.Map<CheckpointDto>(checkpoint);

            var newTitle = "newTitle";
            var expectedCheckpoint = checkpoint;
            expectedCheckpoint.Title = newTitle;

            //mock
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
    }
}
