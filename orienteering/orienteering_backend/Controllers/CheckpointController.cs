using MediatR;
using Microsoft.AspNetCore.Mvc;
using orienteering_backend.Core.Domain.Checkpoint;
using orienteering_backend.Core.Domain.Checkpoint.Dto;
using orienteering_backend.Core.Domain.Checkpoint.Pipelines;
using orienteering_backend.Core.Domain.Navigation.Dto;

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

        //create checkpoint
        //POST
        [HttpPost("createCheckpoint")]
        public async Task<Guid> CreateCheckpoint(CheckpointDto checkpointDto)
        {
            //fiks objekt her i parameter

            var newCheckPointId = await _mediator.Send(new CreateCheckpoint.Request(checkpointDto));

            return newCheckPointId;
        }


        [HttpGet("getCheckpoints")]
        //get all checkpoints that belongs to a track
        public async Task<List<CheckpointDto>> GetCheckpointsOfTrack(string trackId)
        {

            Guid trackGuid = new Guid(trackId);
            var checkpoints = await _mediator.Send(new GetCheckpointsForTrack.Request(trackGuid));
            return checkpoints;

        }


        [HttpGet("getCheckpoint")]
        public async Task<CheckpointDto> GetSingleCheckpoint(string checkpointId)
        {

            Guid CheckpointId = new Guid(checkpointId);
            CheckpointDto checkpoint = await _mediator.Send(new GetSingleCheckpoint.Request(CheckpointId));

            return checkpoint;

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
                return Unauthorized("Could not find the checkpoint to edit");
            }

        }




    }
}
