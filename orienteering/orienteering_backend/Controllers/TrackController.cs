using MediatR;
using Microsoft.AspNetCore.Mvc;
using orienteering_backend.Core.Domain.Track.Pipelines;
using orienteering_backend.Core.Domain.Track;
using Microsoft.AspNetCore.Identity;

namespace orienteering_backend.Controllers
{
    [ApiController]
    [Route("api/track")]
    public class TrackController : ControllerBase
    {
        private readonly IMediator _mediator;
        public TrackController(IMediator Mediator)
        {
            _mediator = Mediator;
        }

        //[HttpGet]
        //public async Task<Guid> Get()
        //{
        //    //Fiks fakeGuid
        //    var fakeGuid = Guid.NewGuid();
        //    var newTrackId = await _mediator.Send(new CreateTrack.Request(fakeGuid));
        //    return newTrackId;

        //}

        [HttpPost("createTrack")]
        public async Task<Guid> CreateTrack(IdentityUser userInfo)
        {
            //fiks objekter her. lage et nytt?? vil sende id men må sende objekt
            //convert from string to guid
            Console.WriteLine($"i create track, userid før behandling er {userInfo.Id}");
            var userGuid = new Guid(userInfo.Id);

            //Fiks fakeGuid-->userGuid
            //var fakeGuid = Guid.NewGuid();
            var newTrackId = await _mediator.Send(new CreateTrack.Request(userGuid));
            return newTrackId;
        }

        [HttpGet("createcheckpoint")]
        public async Task<Guid> CreateCheckpoint(Guid TrackId)
        {
            var newCheckPointId = await _mediator.Send(new CreateCheckpoint.Request(TrackId));

            return newCheckPointId;
        }

        [HttpGet("gettracks")]
        public async Task<List<Track>> GetTracksByUserId(Guid UserId)
        {
            var tracks = await _mediator.Send(new GetTrack.Request(UserId));
            return tracks;

        }


    }
}
