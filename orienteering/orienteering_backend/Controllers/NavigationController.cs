using MediatR;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;
using orienteering_backend.Core.Domain.Checkpoint.Dto;
using orienteering_backend.Core.Domain.Checkpoint.Pipelines;
using orienteering_backend.Core.Domain.Navigation.Dto;
using orienteering_backend.Core.Domain.Navigation.Pipelines;
using SixLabors.ImageSharp;

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
            Guid checkpointGuid = new(checkpointId);

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

            await _mediator.Send(new CreateNavigationImage.Request(path, checkpointGuid));
            
            return Ok();
        }

        [HttpGet("GetNavigation")]
        public async Task<Object> GetNavigation(string checkpointId)
        {
            Guid checkpointGuid = new(checkpointId);
            var res=await _mediator.Send(new GetNavigation.Request(checkpointGuid));
            return new Tull(res);
            //return Ok();
        }
    }


    public class Tull
    {
        public Tull(Image img)
        {
            this.ImageTest = img;
        }

        public Image ImageTest { get; set; }
    }
}
