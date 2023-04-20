using MediatR;
using Microsoft.AspNetCore.Mvc;
using orienteering_backend.Core.Domain.Checkpoint.Dto;
using orienteering_backend.Core.Domain.Checkpoint.Pipelines;
using System.Security.Authentication;

// Lisens MediatR: https://github.com/jbogard/MediatR/blob/master/LICENSE

namespace orienteering_backend.Controllers
{
    [ApiController]
    [Route("api/checkpoint")]
    public class CheckpointController : Controller
    {
        private readonly IMediator _mediator;

        public CheckpointController(IMediator Mediator)
        {
            _mediator = Mediator;
        }

        [HttpPost("createCheckpoint")]
        public async Task<ActionResult> CreateCheckpoint(CheckpointDto checkpointDto)
        {
            if (!ModelState.IsValid) { return BadRequest(ModelState); }

            try
            {
                var newCheckPointId = await _mediator.Send(new CreateCheckpoint.Request(checkpointDto));
                return Ok(newCheckPointId);
            }
            catch
            {
                return NotFound();
            }
        }


        [HttpGet("getCheckpoints")]
        public async Task<ActionResult<List<CheckpointDto>>> GetCheckpointsOfTrack(string trackId)
        {
            if (!ModelState.IsValid) { return BadRequest(ModelState); }
            //check that signed in

            try
            {
                Guid trackGuid = new Guid(trackId);
                var checkpoints = await _mediator.Send(new GetCheckpointsForTrack.Request(trackGuid));
                return Ok(checkpoints);
            }
            catch
            {
                return Unauthorized();
            }
        }

        [HttpGet("getDescription")]
        public async Task<string> GetDescription(string checkpointId)
        {
            var checkpointIdGuid = new Guid(checkpointId);
            var checkpointDescription = await _mediator.Send(new GetCheckpointDescription.Request(checkpointIdGuid));
            return checkpointDescription;
        }

        [HttpGet("getCheckpoint")]
        public async Task<ActionResult<CheckpointDto>> GetSingleCheckpoint(string checkpointId)
        {
            if (!ModelState.IsValid) { return BadRequest(ModelState); }
            Guid CheckpointId = new Guid(checkpointId);

            try
            {
                CheckpointDto checkpoint = await _mediator.Send(new GetSingleCheckpoint.Request(CheckpointId));
                return checkpoint;
            }
            catch(AuthenticationException)
            {
                //not signed in
                return Unauthorized();
            }
            catch(ArgumentNullException)
            {
                //not authenticated or dont exist
                return NotFound();
            }
        }

        [HttpDelete("removeCheckpoint")]
        public async Task<IActionResult> DeleteCheckpoint(string checkpointId)
        {
            if (!ModelState.IsValid) { return BadRequest(ModelState); }
            Guid CheckpointId = new Guid(checkpointId);

            try
            {
                await _mediator.Send(new DeleteCheckpoint.Request(CheckpointId));
                return Ok();
            }
            catch (AuthenticationException)
            {
                //not signed in
                return Unauthorized();
            }
            catch
            {
                //not allowed or dont exist
                return NotFound("Could not find the checkpoint to delete");
            }
        }


        [HttpPatch("editCheckpointTitle")]
        public async Task<IActionResult> UpdateCheckpointTitle(UpdateCheckpointTitleDto checkpointInfo)
        {
            if (!ModelState.IsValid) { return BadRequest(ModelState); }

            try
            {
                var changed = await _mediator.Send(new UpdateCheckpointTitle.Request(checkpointInfo.Title, checkpointInfo.CheckpointId));
                return Ok();
            }
            catch (AuthenticationException)
            {
                //not signed in
                return Unauthorized("Not signed in");
            }
            catch(ArgumentNullException)
            {
                //does not exist or not allowed
                return NotFound("Could not find the checkpoint to edit");
            }
        }

        [HttpGet("getqrcodes")]
        public async Task<ActionResult<List<CheckpointNameAndQRCodeDto>>> GetQRCodes(string TrackId)
        {
            if (!ModelState.IsValid) { return BadRequest(ModelState); }

            var track = new Guid(TrackId);

            try
            {
                var checkpointList = await _mediator.Send(new GetQRCodes.Request(track));
                return checkpointList;
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

        [HttpGet("getGameidForCheckpoint")]
        public async Task<ActionResult<int>> getGameidForCheckpoint(string checkpointId)
        {
            if (!ModelState.IsValid) { return BadRequest(ModelState); }
            var checkpointGuid=new Guid(checkpointId);

            try
            {
                var gameId = await _mediator.Send(new GetGameIdOfCheckpoint.Request(checkpointGuid));
                return Ok(gameId);
            }
            catch
            {
                return NotFound();
            }
        }
    }
}
