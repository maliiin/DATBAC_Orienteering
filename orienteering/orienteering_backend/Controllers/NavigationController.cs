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

          

            

            await _mediator.Send(new CreateNavigationImage.Request(checkpointGuid, file));

            return Ok();
        }



        [HttpGet("GetNavigation")]
        public async Task<NavigationDto> GetNavigation(string checkpointId)
        {
            Guid checkpointGuid = new(checkpointId);
            NavigationDto navDto=await _mediator.Send(new GetNavigation.Request(checkpointGuid));
            return navDto;
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
