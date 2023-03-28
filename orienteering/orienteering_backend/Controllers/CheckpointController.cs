﻿using MediatR;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;
using orienteering_backend.Core.Domain.Authentication.Services;
using orienteering_backend.Core.Domain.Checkpoint.Dto;
using orienteering_backend.Core.Domain.Checkpoint.Pipelines;

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
            var userId = _identityService.GetCurrentUserId();
            if (userId == null) { return Unauthorized(); }

            //fiks objekt her i parameter (tror ok?)
            try
            {
                var newCheckPointId = await _mediator.Send(new CreateCheckpoint.Request(checkpointDto, (Guid)userId));
                return Ok(newCheckPointId);
            }
            catch
            {
                return Unauthorized();
            }

        }


        [HttpGet("getCheckpoints")]
        //get all checkpoints that belongs to a track
        public async Task<ActionResult<List<CheckpointDto>>> GetCheckpointsOfTrack(string trackId)
        {
            //check that signed in
            var userId = _identityService.GetCurrentUserId();
            if (userId == null) { return Unauthorized(); }

            try
            {
                Guid trackGuid = new Guid(trackId);
                var checkpoints = await _mediator.Send(new GetCheckpointsForTrack.Request(trackGuid, (Guid)userId));
                return Ok(checkpoints);

            }
            catch
            {
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
            catch
            {
                return Unauthorized();
            }


        }
        //sjekk om db order blir autoinkrementet av nytt checkpoint

        [HttpDelete("removeCheckpoint")]
        public async Task<IActionResult> DeleteCheckpoint(string checkpointId)
        {
            Console.WriteLine("prøver å slettw");
            Guid CheckpointId = new Guid(checkpointId);
            bool removed = await _mediator.Send(new DeleteCheckpoint.Request(CheckpointId));
            if (removed)
            {
                return Ok();

            }
            else
            {
                return NotFound("Could not find the checkpoint to delete");
            }

        }

        [HttpPut("editCheckpointTitle")]
        public async Task<IActionResult> UpdateCheckpointTitle(string checkpointTitle, string checkpointId)
        {
            Guid CheckpointId = new Guid(checkpointId);

            var changed = await _mediator.Send(new UpdateCheckpointTitle.Request(checkpointTitle, CheckpointId));
            if (changed != null)
            {
                return Ok();

            }
            else
            {
                //fix feilmelding
                return NotFound("Could not find the checkpoint to edit");
            }
        }
    }
}
