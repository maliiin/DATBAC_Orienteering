using MediatR;
using Microsoft.AspNetCore.Mvc;
using orienteering_backend.Core.Domain.Track.Pipelines;
using orienteering_backend.Core.Domain.Track.Dto;
using System.Security.Authentication;

// Lisens MediatR: https://github.com/jbogard/MediatR/blob/master/LICENSE

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

        [HttpPatch("updateTrackTitle")]
        public async Task<IActionResult> UpdateTrackTitle(UpdateTrackTitleDto updateData)
        {
            try
            {
                await _mediator.Send(new UpdateTrackTitle.Request(updateData.TrackId, updateData.NewTitle));
                return Ok();

            }
            catch (AuthenticationException)
            {
                return Unauthorized();
            }
            catch (NullReferenceException)
            {
                return NotFound();
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
            catch (AuthenticationException)
            {
                return Unauthorized();
            }
            catch (NullReferenceException)
            {
                return NotFound();
            }
        }

    }

}
