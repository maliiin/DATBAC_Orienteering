using AutoMapper;
using MediatR;
using Microsoft.EntityFrameworkCore;
using Moq;
using Newtonsoft.Json;
using orienteering_backend.Core.Domain.Authentication.Services;
using orienteering_backend.Core.Domain.Checkpoint;
using orienteering_backend.Core.Domain.Checkpoint.Dto;
using orienteering_backend.Core.Domain.Checkpoint.Pipelines;
using orienteering_backend.Core.Domain.Navigation;
using orienteering_backend.Core.Domain.Navigation.Pipelines;
using orienteering_backend.Core.Domain.Track;
using orienteering_backend.Core.Domain.Track.Dto;
using orienteering_backend.Core.Domain.Track.Pipelines;
using orienteering_backend.Infrastructure.Automapper;
using orienteering_backend.Infrastructure.Data;

//fix-generelt på tester-er det best å bruke moq mapper eller _mapper?
//sjekk at vi er konsekvente
namespace orienteering_backend.Tests.Helpers
{
    public class NavigationTest
    {
        public readonly DbContextOptions<OrienteeringContext> dbContextOptions;

        private static IMapper _mapper;
        public NavigationTest()
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



        //mal
        [Fact]
        public async Task GivenCorrectUser_WhenUpdateNavigationText_ThenUpdateText()
        {
            //ARRANGE
            var _db = new OrienteeringContext(dbContextOptions);
            if (!_db.Database.IsInMemory()) { _db.Database.Migrate(); }

            var userId=Guid.NewGuid();
            var newDescription = "new text";

            //create track
            var track = new Track();
            track.Name = "Test";
            track.UserId = userId;
            await _db.Tracks.AddAsync(track);
            await _db.SaveChangesAsync();

            var trackUserDto = _mapper.Map<TrackUserIdDto>(track);

            //create checkpoint
            var checkpoint = new Checkpoint("test1", 0, track.Id);
            await _db.Checkpoints.AddAsync(checkpoint);
            await _db.SaveChangesAsync();

            var checkpointDto = _mapper.Map<CheckpointDto>(checkpoint);


            //create navigation
            var navigationImage = new NavigationImage("fakePath/fakeFile.jpg", 1, "go to the left");
            var navigation = new Navigation(checkpoint.Id);
            navigation.AddNavigationImage(navigationImage);
            await _db.Navigation.AddAsync(navigation);
            await _db.SaveChangesAsync();

            //mock
            var _identityService = new Mock<IIdentityService>();
            _identityService.Setup(i => i.GetCurrentUserId()).Returns(userId);

            var _mediator = new Mock<IMediator>();
            _mediator.Setup(m => m.Send(It.IsAny<GetSingleCheckpoint.Request>(), It.IsAny<CancellationToken>())).ReturnsAsync(checkpointDto);
            _mediator.Setup(m => m.Send(It.IsAny<GetTrackUser.Request>(), It.IsAny<CancellationToken>())).ReturnsAsync(trackUserDto);



            var request = new UpdateNavigationText.Request(navigation.Id, newDescription, navigationImage.Id);
            var handler = new UpdateNavigationText.Handler(_db, _identityService.Object, _mediator.Object);

            //ACT

            var response = handler.Handle(request, CancellationToken.None).GetAwaiter().GetResult();
            var navigationDb=await _db.Navigation.Where(n=>n.Id== navigation.Id).Include(n=>n.Images).FirstOrDefaultAsync();
            
            navigation.Images[0].TextDescription = newDescription;

            //fix denne testen, sjekk at db nav er ok i forhold til forventet nav
            //tror test ok
            //ASSERT
            Assert.Equal(JsonConvert.SerializeObject(navigation),JsonConvert.SerializeObject(navigationDb));
            Assert.Equal(JsonConvert.SerializeObject(navigation.Images[0]), JsonConvert.SerializeObject(navigationDb.Images[0]));

        }
    }
}
