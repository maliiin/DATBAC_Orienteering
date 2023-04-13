using MediatR;
using Microsoft.AspNetCore.Mvc;
using orienteering_backend.Core.Domain.Track.Pipelines;
using orienteering_backend.Core.Domain.Track.Dto;

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

        //greate new track
        //POST
        // [Authorize]
        [HttpPost("createTrack")]
        public async Task<ActionResult> CreateTrack(CreateTrackDto trackDto)
        {
            //tar ikke in userid lenger, det skal backend hente inn selv
            //fiks det i pipelinen
            //sjekk at det virker etterpå
            try
            {
                var newTrackId = await _mediator.Send(new CreateTrack.Request(trackDto));
                return Ok();
            }
            catch
            {
                //not signed in
                return Unauthorized();
            }

        }


        //list of all tracks of a user
        [HttpGet("getTracks")]
        public async Task<ActionResult<List<TrackDto>>> GetTracksForUser()
        {
            try
            {
                var tracks = await _mediator.Send(new GetTracks.Request());
                return Ok(tracks);
            }
            catch
            {
                return Unauthorized();
            }

        }


        [HttpGet("getTrack")]
        public async Task<ActionResult<TrackDto>> GetSingleTrack(string trackId)
        {
            try
            {
                Guid trackGuid = new Guid(trackId);
                TrackDto trackDto = await _mediator.Send(new GetSingleTrack.Request(trackGuid));
                return trackDto;
            }
            catch
            {
                return Unauthorized();
            }


        }

        //fiks-sjekk metode put eller patch
        [HttpPut("updateTrackTitle")]
        public async Task<IActionResult> UpdateTrackTitle(string trackId, string newTitle)
        {
            Guid trackGuid = new Guid(trackId);
            try
            {
                bool response = await _mediator.Send(new UpdateTrackTitle.Request(trackGuid, newTitle));
                //fiks sjekk respons
                if (response) { return Ok(); }
                else { return NotFound("could not find the track to update"); }
            }
            catch
            {
                return Unauthorized();
            }

        }


        [HttpDelete("deleteTrack")]
        public async Task<IActionResult> DeleteTrack(string trackId)
        {
            Guid trackGuid = new Guid(trackId);
            try
            {
                //fiks sjekk respons
                bool response = await _mediator.Send(new DeleteTrack.Request(trackGuid));
                return Ok();
            }
            catch { return Unauthorized(); }
        }

    }

}
