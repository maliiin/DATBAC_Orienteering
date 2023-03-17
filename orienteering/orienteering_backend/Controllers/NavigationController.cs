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
            //blob test
            //https://stackoverflow.com/questions/73026716/fetch-image-from-c-sharp-web-api
            Guid checkpointGuid = new(checkpointId);
            var res=await _mediator.Send(new GetNavigation.Request(checkpointGuid));
            return res;

            //string path = Path.Combine(Directory.GetCurrentDirectory(), "wwwroot", "08db26c5-298c-4398-847c-7d9ad2136e02", "a bilde.png");
            //byte[] ret;

            ////fix usikker på om fileshare
            //using (FileStream t = System.IO.File.Open(path, FileMode.Open, FileAccess.Read ,FileShare.Read))
            //{
            //    ret= System.IO.File.ReadAllBytes(path);

            //}
            ////https://stackoverflow.com/questions/26741191/ioexception-the-process-cannot-access-the-file-file-path-because-it-is-being

            ////return t;
            ////return new Tull(t);
            ////return Ok();

            ////byte[] bytes = System.IO.File.ReadAllBytes(path);
            //return new Tull2(ret);



        }
    }





    //fix slett
    public class Tull2
    {
        public Tull2(byte[] img)
        {
            ImageTest = img;
        }

        //public Image ImageTest { get; set; }
        public byte[] ImageTest { get; set; }
    }



}
