using MediatR;
using Microsoft.AspNetCore.Mvc;
using orienteering_backend.Core.Domain.Checkpoint.Pipelines;
using orienteering_backend.Core.Domain.Navigation.Dto;
using orienteering_backend.Core.Domain.Navigation.Pipelines;
using System.Security.Authentication;

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

        //kilde legg kode i kontrolleren
        //får status 200 nå
        //https://stackoverflow.com/questions/55205135/how-to-upload-image-from-react-to-asp-net-core-web-api
        //https://sankhadip.medium.com/how-to-upload-files-in-net-core-web-api-and-react-36a8fbf5c9e8

        [HttpPost("AddImage")]
        public async Task<ActionResult> AddImage([FromForm] IFormFile file)
        {
            var checkpointId = HttpContext.Request.Form["checkpointId"];
            var textDescription = HttpContext.Request.Form["textDescription"];
            Guid checkpointGuid = new(checkpointId);

            try
            {
                await _mediator.Send(new CreateNavigationImage.Request(checkpointGuid, file, textDescription));
                return Ok();

            }
            catch (AuthenticationException)
            {
                return Unauthorized();
            }
            catch (NullReferenceException)
            {
                return NotFound();
            }
        }

        [HttpDelete("DeleteImage")]
        public async Task<ActionResult> DeleteImage(string navigationId, string imageId)
        {
            Guid imageGuid = new(imageId);
            Guid navigationGuid = new(navigationId);

            try
            {
                await _mediator.Send(new NavigationDeleteImage.Request(imageGuid, navigationGuid));
                return Ok();
            }
            catch (AuthenticationException)
            {
                return Unauthorized();
            }
            catch (NullReferenceException)
            {
                return NotFound();
            }
        }


        [HttpGet("GetNavigation")]
        public async Task<ActionResult<NavigationDto>> GetNavigation(string checkpointId)
        {
            Guid checkpointGuid = new(checkpointId);

            try
            {
                NavigationDto navDto = await _mediator.Send(new GetNavigation.Request(checkpointGuid));
                return navDto;
            }
            catch (AuthenticationException)
            {
                return Unauthorized();
            }
            catch (NullReferenceException)
            {
                return NotFound();
            }

        }


        //denne skal ikke være beskyttet
        [HttpGet("GetNextNavigation")]
        public async Task<NavigationDto> GetNavigationForNextCheckpoint(string currentCheckpointId)
        {
            Guid currentCheckpointGuid = new Guid(currentCheckpointId);
            var nextCheckpointId = await _mediator.Send(new GetNextCheckpoint.Request(currentCheckpointGuid));
            var navDto = await _mediator.Send(new GetNavigationUnauthorized.Request(nextCheckpointId));

            return navDto;
        }

        [HttpPut("editNavigationText")]
        public async Task<IActionResult> UpdateNavigationDescription(string navigationId, string newText, string navigationImageId)
        {
            Guid NavigationId = new Guid(navigationId);
            Guid NavigationImageId = new Guid(navigationImageId);

            try
            {
                var changed = await _mediator.Send(new UpdateNavigationText.Request(NavigationId, newText, NavigationImageId));
                return Ok();
            }
            catch
            {
                //fix feilmelding
                return Unauthorized("Could not find the navigation to edit");
            }
        }
    }
}
