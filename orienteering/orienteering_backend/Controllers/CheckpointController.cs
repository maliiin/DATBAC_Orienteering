using MediatR;
using Microsoft.AspNetCore.Mvc;
using orienteering_backend.Core.Domain.Authentication.Services;
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
        private readonly IIdentityService _identityService;

        public CheckpointController(IMediator Mediator, IIdentityService identityService)
        {
            _mediator = Mediator;
            _identityService = identityService;
        }

        //[Authorize]
        [HttpPost("createCheckpoint")]
        public async Task<ActionResult> CreateCheckpoint(CheckpointDto checkpointDto)
        {
            //fiks objekt her i parameter (tror ok?)
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
            try
            {
                Guid trackGuid = new Guid(trackId);
                var checkpoints = await _mediator.Send(new GetCheckpointsForTrack.Request(trackGuid));
                return Ok(checkpoints);
            }
            catch
            {
                //fix-eller skal den returnere NotFound
                //user dont own this track
                return Unauthorized();
            }
        }

        [HttpGet("getCheckpoint")]
        public async Task<ActionResult<CheckpointDto>> GetSingleCheckpoint(string checkpointId)
        {
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
            catch(NullReferenceException)
            {
                //not authenticated or dont exist
                return NotFound();
            }
        }
        //sjekk om db order blir autoinkrementet av nytt checkpoint

        [HttpDelete("removeCheckpoint")]
        public async Task<IActionResult> DeleteCheckpoint(string checkpointId)
        {
            Guid CheckpointId = new Guid(checkpointId);

            try
            {
                bool removed = await _mediator.Send(new DeleteCheckpoint.Request(CheckpointId));
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
            catch(NullReferenceException)
            {
                //does not exist or not allowed
                return NotFound("Could not find the checkpoint to edit");
            }
        }
    }
}
