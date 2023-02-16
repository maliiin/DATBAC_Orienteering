using MediatR;
using Microsoft.AspNetCore.Mvc;
using orienteering_backend.Core.Domain.Track.Pipelines;
using orienteering_backend.Core.Domain.Authentication;
using Microsoft.AspNetCore.Identity;
using orienteering_backend.Core.Domain.Track.Services;
using orienteering_backend.Core.Domain.Track.Dto;
using orienteering_backend.Core.Domain.Checkpoint.Dto;
using orienteering_backend.Core.Domain.Checkpoint.Pipelines;
using orienteering_backend.Core.Domain.Checkpoint;
using Microsoft.AspNetCore.Authorization;

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
       // [Authorize]
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
        public async Task<List<TrackDto>> GetTracksByUserId(string userId)
        {
            var UserId =  new Guid(userId);
            var tracks = await _mediator.Send(new GetTracks.Request(UserId));
            return tracks;
        }

        [HttpGet("getCheckpoints")]
        public async Task<List<CheckpointDto>> 
            OfTrack(string trackId)
        {

            Guid trackGuid= new Guid(trackId);
            var checkpoints = await _trackService.GetCheckpointsForTrack(trackGuid);
            return checkpoints;

        }

        [HttpGet("getTrack")]
        public async Task<TrackDto> GetSingleTrack(string trackId)
        {

            Guid trackGuid = new Guid(trackId);
            TrackDto trackDto = await _mediator.Send(new GetSingleTrack.Request(trackGuid));

            return trackDto;

        }

        [HttpGet("getCheckpoint")]
        public async Task<CheckpointDto> GetSingleCheckpoint(string checkpointId)
        {

            Guid CheckpointId = new Guid(checkpointId);
            CheckpointDto checkpoint = await _mediator.Send(new GetSingleCheckpoint.Request(CheckpointId));

            return checkpoint;

        }
    }

}
