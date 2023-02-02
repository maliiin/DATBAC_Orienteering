using MediatR;
using Microsoft.AspNetCore.Mvc;
using orienteering_backend.Core.Domain.Track.Pipelines;

namespace orienteering_backend.Controllers
{
    [ApiController]
    [Route("track")]
    public class TrackController : ControllerBase
    {
        private readonly IMediator Mediator;
        public TrackController(IMediator mediator)
        {
            Mediator = mediator;
        }

        [HttpGet]
        public async Task<Guid> Get()
        {
            //Fiks fakeGuid
           var fakeGuid = Guid.NewGuid();
            var newTrack = await Mediator.Send(new CreateTrack.Request(fakeGuid));
            return newTrack;

        }
    }
}
