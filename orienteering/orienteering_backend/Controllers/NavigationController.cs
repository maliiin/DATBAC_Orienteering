using MediatR;
using Microsoft.AspNetCore.Mvc;
using orienteering_backend.Core.Domain.Checkpoint.Pipelines;
using orienteering_backend.Core.Domain.Navigation.Dto;
using orienteering_backend.Core.Domain.Navigation.Pipelines;
using System.Security.Authentication;

// Lisens MediatR: https://github.com/jbogard/MediatR/blob/master/LICENSE

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

        [HttpPost("AddImage")]
        public async Task<ActionResult> AddImage([FromForm] IFormFile file)
        {
            if (!ModelState.IsValid) { return BadRequest(ModelState); }

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
            catch (ArgumentNullException)
            {
                return NotFound();
            }
        }

        [HttpDelete("DeleteImage")]
        public async Task<ActionResult> DeleteImage(string navigationId, string imageId)
        {
            if (!ModelState.IsValid) { return BadRequest(ModelState); }

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
            catch (ArgumentNullException)
            {
                return NotFound();
            }
        }


        [HttpGet("GetNavigation")]
        public async Task<ActionResult<NavigationDto>> GetNavigation(string checkpointId)
        {
            if (!ModelState.IsValid) { return BadRequest(ModelState); }

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
            catch (ArgumentNullException)
            {
                return NotFound();
            }

        }


        [HttpGet("GetNextNavigation")]
        public async Task<ActionResult<NavigationDto>> GetNavigationForNextCheckpoint(string currentCheckpointId)
        {
            if (!ModelState.IsValid) { return BadRequest(ModelState); }

            Guid currentCheckpointGuid = new Guid(currentCheckpointId);
            var nextCheckpointId = await _mediator.Send(new GetNextCheckpoint.Request(currentCheckpointGuid));
            var navDto = await _mediator.Send(new GetNavigationUnauthorized.Request(nextCheckpointId));

            return navDto;
        }

        [HttpPatch("editNavigationText")]
        public async Task<IActionResult> UpdateNavigationDescription(UpdateNavigationTextDto NavigationInfo)
        {
            if (!ModelState.IsValid) { return BadRequest(ModelState); }

            try
            {
                var changed = await _mediator.Send(new UpdateNavigationText.Request(NavigationInfo.NavigationId, NavigationInfo.NewText, NavigationInfo.NavigationImageId));
                return Ok();
            }
            catch (AuthenticationException)
            {
                return Unauthorized();
            }
            catch (ArgumentNullException)
            {
                return NotFound();
            }
        }
    }
}
