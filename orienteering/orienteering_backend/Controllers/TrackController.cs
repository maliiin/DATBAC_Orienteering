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

        [HttpPost("createTrack")]
        public async Task<ActionResult> CreateTrack(CreateTrackDto trackDto)
        {
            if (!ModelState.IsValid) { return BadRequest(ModelState); }

            try
            {
                var newTrackId = await _mediator.Send(new CreateTrack.Request(trackDto));
                return Ok();
            }
            catch
            {
                return Unauthorized();
            }
        }


        [HttpGet("getTracks")]
        public async Task<ActionResult<List<TrackDto>>> GetTracksForUser()
        {
            if (!ModelState.IsValid) { return BadRequest(ModelState); }

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
            if (!ModelState.IsValid) { return BadRequest(ModelState); }

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
        public async Task<IActionResult> UpdateTrackTitle(UpdateTrackTitleDto TrackInfo)
        {
            if (!ModelState.IsValid) { return BadRequest(ModelState); }

            try
            {
                await _mediator.Send(new UpdateTrackTitle.Request(TrackInfo.TrackId, TrackInfo.NewTitle));
                return Ok();

            }
            catch (AuthenticationException)
            {
                return Unauthorized();
            }
            catch (ArgumentNullException)
            {
                return NotFound();
            }

        }


        [HttpDelete("deleteTrack")]
        public async Task<IActionResult> DeleteTrack(string trackId)
        {
            if (!ModelState.IsValid) { return BadRequest(ModelState); }

            Guid trackGuid = new Guid(trackId);
            try
            {
                await _mediator.Send(new DeleteTrack.Request(trackGuid));
                return Ok();
            }
            catch (AuthenticationException)
            {
                return Unauthorized();
            }
            catch (ArgumentNullException)
            {
                return NotFound();
            }
        }

    }

}
