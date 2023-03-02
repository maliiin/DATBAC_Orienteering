using MediatR;
using Microsoft.AspNetCore.Mvc;
using orienteering_backend.Core.Domain.Checkpoint.Dto;
using orienteering_backend.Core.Domain.Checkpoint.Pipelines;

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


    }
}
