using MediatR;
using Microsoft.AspNetCore.Mvc;
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

        //legg kode i kontrolleren
        //får status 200 nå
        //https://stackoverflow.com/questions/55205135/how-to-upload-image-from-react-to-asp-net-core-web-api
        //https://sankhadip.medium.com/how-to-upload-files-in-net-core-web-api-and-react-36a8fbf5c9e8

        [HttpPost("AddImage")]
        public async Task<ActionResult> AddImage([FromForm] IFormFile file)
        {
            //Console.WriteLine("nå her\n\n\n");
            //Console.WriteLine(file.FileName);
            //Console.WriteLine(file.FormFile);

            //var testnavn = file.FileName;
            //Console.WriteLine(testnavn);



            string extension = Path.GetExtension(file.FileName);
            //read the file
            string path = Path.Combine(Directory.GetCurrentDirectory(), "wwwroot", file.FileName);
            using (var memoryStream = new FileStream(path, FileMode.Create))
            {
                file.CopyTo(memoryStream);
            }

            //denne virker alene
            //using (var memoryStream = new MemoryStream())
            //{
            //    file.CopyTo(memoryStream);
            //}


            return Ok();
            ////tru catch kilde https://sankhadip.medium.com/how-to-upload-files-in-net-core-web-api-and-react-36a8fbf5c9e8 16/03
            //try
            //{
            //    string path = Path.Combine(Directory.GetCurrentDirectory(), "../wwwroot", file.FileName);

            //    using (Stream stream = new FileStream(path, FileMode.Create))
            //    {
            //        file.FormFile.CopyTo(stream);
            //    }

            //    return Ok();
            //}
            //catch
            //{
            //    return StatusCode(StatusCodes.Status402PaymentRequired);

            //    //return StatusCode(StatusCodes.Status500InternalServerError);
            //}

        }

        [HttpGet("test")]
        public ActionResult test(string formFile)
        {
            Console.WriteLine(formFile);
            return Ok(formFile);

        }





        //FIX FLYTT DET IVER


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
