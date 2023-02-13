using MediatR;
using Microsoft.AspNetCore.Mvc;
using orienteering_backend.Core.Domain.Track.Pipelines;
using orienteering_backend.Core.Domain.Track;
using orienteering_backend.Core.Domain.Authentication;
using Microsoft.AspNetCore.Identity;
using orienteering_backend.Core.Domain.Track.Services;
using orienteering_backend.Core.Domain.Track.Dto;

namespace orienteering_backend.Controllers
{
    [ApiController]
    [Route("api/track")]
    public class TrackController : ControllerBase
    {
        private readonly IMediator _mediator;
        private readonly ITrackService _trackService;

        public TrackController(IMediator Mediator, ITrackService trackService)
        {
            _mediator = Mediator;
            _trackService = trackService;
        }
        //bør userGuid sendes inn fra frontend? eller skal backend hente userId fra seg selv fra den som er logget inn?

        //greate new track
        //POST
        [HttpPost("createTrack")]
        public async Task<Guid> CreateTrack(TrackDto trackDto)
        {
            var newTrackId = await _mediator.Send(new CreateTrack.Request(trackDto));
            return newTrackId;
        }




        //create checkpoint
        //POST
        [HttpPost("createCheckpoint")]
        public async Task<Guid> CreateCheckpoint(CheckpointDto checkpointDto)
        {
            //fiks objekt her i parameter
            
            //Guid TrackId =new Guid(track.Id);
            //Console.WriteLine("inni create checkpoint");
            var newCheckPointId = await _mediator.Send(new CreateCheckpoint.Request(checkpointDto));

            return newCheckPointId;
        }



        
        //list of all tracks of a user
        [HttpGet("getTracks")]
        public async Task<Array> GetTracksByUserId(string userId)
        {
            //Guid UserId = new Guid(userInfo.Id);
            Console.WriteLine($"\n\nuser id før tracksUserId {userId}\n\n");

            var UserId =  new Guid(userId);
            var tracks = await _mediator.Send(new GetTrack.Request(UserId));
            Console.WriteLine($"\n\n\n{tracks}");
            //Console.WriteLine(tracks.Count);
            Console.WriteLine(tracks.Length);
            Console.WriteLine("slutt");
            return tracks;
        }

        [HttpGet("getCheckpoints")]
        public async Task<List<Checkpoint>> GetCheckponitsOfTrack(string trackId)
        {

            Guid trackGuid= new Guid(trackId);
            var checkpoints = await _trackService.GetCheckponitsForTrack(trackGuid);
            return checkpoints;

        }
    }


    public class Tester
    {
        public string Id { get; set; }
    }
}
