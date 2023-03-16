using MediatR;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;
using orienteering_backend.Core.Domain.Checkpoint.Dto;
using orienteering_backend.Core.Domain.Checkpoint.Pipelines;
using orienteering_backend.Core.Domain.Navigation.Dto;

namespace orienteering_backend.Controllers
{
    [ApiController]
    [Route("api/navigation")]

    public class NavigationController : Controller
    {
        private readonly IMediator _mediator;

        public NavigationController(IMediator mediator)
        {
            _mediator = mediator;
        }




        //legg kode i kontrolleren
        //får status 200 nå
        //https://stackoverflow.com/questions/55205135/how-to-upload-image-from-react-to-asp-net-core-web-api
        //https://sankhadip.medium.com/how-to-upload-files-in-net-core-web-api-and-react-36a8fbf5c9e8

        [HttpPost("AddImage")]
        public async Task<ActionResult> AddImage([FromForm] IFormFile file)
        {

            var checkpointId = HttpContext.Request.Form["checkpointId"];

            //string extension = Path.GetExtension(file.FileName);
            //Console.WriteLine(file);

            //read the file
            string localPath = $"{checkpointId}";
            //wwwroot/checkpointId
            string dirPath = Path.Combine(Directory.GetCurrentDirectory(), "wwwroot", localPath);
            //create wwwroot/checkpointId dir if not exists
            if (!Directory.Exists(dirPath))
            {
                Directory.CreateDirectory(dirPath);
            }

            //place image in correct place
            string path = Path.Combine(Directory.GetCurrentDirectory(), "wwwroot", localPath, file.FileName);
            using (var memoryStream = new FileStream(path, FileMode.Create))
            {
                file.CopyTo(memoryStream);
            }

            //_mediator.Send(new AddNavigationImage.Request(path,checkpointId))
            
            return Ok();
        }

        [HttpGet("test")]
        public ActionResult test(string formFile)
        {
            Console.WriteLine(formFile);
            return Ok(formFile);
        }
    }
}
