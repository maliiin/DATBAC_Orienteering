using AutoMapper;
using Microsoft.EntityFrameworkCore;
using Moq;
using Newtonsoft.Json;
using orienteering_backend.Core.Domain.Track.Dto;
using orienteering_backend.Core.Domain.Track.Pipelines;
using orienteering_backend.Core.Domain.Track;
using orienteering_backend.Infrastructure.Automapper;
using orienteering_backend.Infrastructure.Data;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using orienteering_backend.Core.Domain.Quiz;
using orienteering_backend.Core.Domain.Quiz.Dto;
using MediatR;
using orienteering_backend.Core.Domain.Quiz.Pipelines;
using orienteering_backend.Core.Domain.Authentication.Services;
using orienteering_backend.Core.Domain.Checkpoint.Dto;
using orienteering_backend.Core.Domain.Checkpoint;
using orienteering_backend.Core.Domain.Checkpoint.Pipelines;
using System.Reflection.Metadata;

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

            // "Mocker" automapper Fix bruker mock nå heller eller ikke?
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
            //arrange
            var _db = new OrienteeringContext(dbContextOptions, null);
            if (!_db.Database.IsInMemory()) { _db.Database.Migrate(); }

            var userId = Guid.NewGuid();
            var trackId = Guid.NewGuid();


            TrackDto trackDto = new();
            trackDto.UserId = userId;
            trackDto.TrackId = trackId;
            trackDto.TrackName = "trackname";

            Checkpoint expectedCheckpoint = new("test1", 0, trackId);
            CheckpointDto checkpointDto = new("test1", trackId, 0);

            var _mediator = new Mock<IMediator>();
            _mediator.Setup(m => m.Send(It.IsAny<GetSingleTrack.Request>(), It.IsAny<CancellationToken>())).ReturnsAsync(trackDto);


            //var userId =Guid.NewGuid();
            //var trackId = new Guid();
            //Checkpoint checkpoint = new("test1", 0, trackId);
            //CheckpointDto checkpointDto = new("test1", trackId, 0);

            var request = new CreateCheckpoint.Request(checkpointDto, userId);
            var handler = new CreateCheckpoint.Handler(_db, _mediator.Object);

            //act
            var response = handler.Handle(request, CancellationToken.None).GetAwaiter().GetResult();
            var allCheckpoint=await _db.Checkpoints.ToListAsync();
            var h = trackDto.TrackId;
            //var createdCheckpoint=await _db.Checkpoints.Where(c => c.TrackId == trackDto.TrackId).FirstOrDefaultAsync();
            //createdCheckpoint.Id = new Guid();
            //assert
            Assert.IsType<Guid>(response);
            //Assert.Equal(JsonConvert.SerializeObject(expectedCheckpoint), JsonConvert.SerializeObject(createdCheckpoint));


            //fix denne testen. created checkpoint inneholder quiz id og sånn som ikke finnes ellers.
            //så vanskelig å sammenligne objektene direkte.

        }


    }
}
