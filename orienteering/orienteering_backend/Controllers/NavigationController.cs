using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;
using orienteering_backend.Core.Domain.Checkpoint.Dto;
using orienteering_backend.Core.Domain.Checkpoint.Pipelines;

namespace orienteering_backend.Controllers
{
    [ApiController]
    [Route("api/navigation")]

    public class NavigationController : Controller
    {
        //legg kode i kontrolleren
        //får status 200 nå
        //https://stackoverflow.com/questions/55205135/how-to-upload-image-from-react-to-asp-net-core-web-api
        //https://sankhadip.medium.com/how-to-upload-files-in-net-core-web-api-and-react-36a8fbf5c9e8

        [HttpPost("AddImage")]
        public async Task<ActionResult> AddImage([FromForm] IFormFile file)
        {
            string extension = Path.GetExtension(file.FileName);
            //read the file
            string path = Path.Combine(Directory.GetCurrentDirectory(), "wwwroot", file.FileName);
            using (var memoryStream = new FileStream(path, FileMode.Create))
            {
                file.CopyTo(memoryStream);
            }

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
