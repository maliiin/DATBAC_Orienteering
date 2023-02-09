using MediatR;
using Microsoft.AspNetCore.Mvc;
using orienteering_backend.Core.Domain.Track.Pipelines;
using orienteering_backend.Core.Domain.Track;
using orienteering_backend.Core.Domain.Authentication;
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
        //bør userGuid sendes inn fra frontend? eller skal backend hente userId fra seg selv fra den som er logget inn?

        //[HttpGet]
        //greate new track
        //POST
        [HttpPost("createTrack")]
        //public async Task<Guid> CreateTrack(Guid userGuid)
        public async Task<Guid> CreateTrack(IdentityUser userInfo)

        {
            //fiks objekter her. lage et nytt?? vil sende id men må sende objekt
            //convert from string to guid
            var userGuid = new Guid(userInfo.Id);

            //Fiks fakeGuid-->userGuid
            //var fakeGuid = Guid.NewGuid();
            var newTrackId = await _mediator.Send(new CreateTrack.Request(userGuid));
            return newTrackId;

        }

        //create checkpoint
        //POST
        //bør ikke dette være post??
        //[HttpGet("createcheckpoint")]
        [HttpPost("createCheckpoint")]
        public async Task<int> CreateCheckpoint(Guid TrackId)
        {
            var newCheckPointId = await _mediator.Send(new CreateCheckpoint.Request(TrackId));
            return newCheckPointId;
        }
        
        //list of all tracks of a user
        //GET
        [HttpGet("getTracks")]
        //public async Task<List<Track>> GetTracksByUserId(string userId)
        public async Task<Array> GetTracksByUserId(string userId)

        {

            //Guid UserId = new Guid(userInfo.Id);
            Console.WriteLine($"\n\nuser id før tracksUserId {userId}\n\n");
            
            var test = new Guid("a7803486-9413-41c1-b85d-6772213ac551");




            var UserId =  new Guid(userId);
            var tracks = await _mediator.Send(new GetTrack.Request(UserId));
            Console.WriteLine($"\n\n\n{tracks}");
            //Console.WriteLine(tracks.Count);
            Console.WriteLine(tracks.Length);
            Console.WriteLine("slutt");
            return tracks;
        }


    }
}
