using MediatR;
using Microsoft.AspNetCore.Mvc;
using orienteering_backend.Core.Domain.Track.Pipelines;

namespace orienteering_backend.Controllers
{
    [ApiController]
    [Route("track")]
    public class TrackController : ControllerBase
    {
        private readonly IMediator _mediator;
        public TrackController(IMediator Mediator)
        {
            _mediator = Mediator;
        }

        [HttpGet]
        public async Task<Guid> Get()
        {
            //Fiks fakeGuid
            var fakeGuid = Guid.NewGuid();
            var newTrack = await _mediator.Send(new CreateTrack.Request(fakeGuid));
            return newTrack;

        }
    }
}
