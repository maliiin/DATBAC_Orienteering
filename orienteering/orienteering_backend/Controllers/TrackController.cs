using MediatR;
using Microsoft.AspNetCore.Mvc;
using orienteering_backend.Core.Domain.Track.Pipelines;
using orienteering_backend.Core.Domain.Track;

namespace orienteering_backend.Controllers
{
    [ApiController]
    [Route("api/track")]
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
            var newTrackId = await _mediator.Send(new CreateTrack.Request(fakeGuid));
            return newTrackId;

        }
        [HttpGet("createcheckpoint")]
        public async Task<Guid> CreateCheckpoint(Guid TrackId)
        {
            var newCheckPointId = await _mediator.Send(new CreateCheckpoint.Request(TrackId));

            return newCheckPointId;
        }

        [HttpGet("gettracks")]
        public async Task<List<Track>> GetTracksByUserId(Guid UserId)
        {
            var tracks = await _mediator.Send(new GetTrack.Request(UserId));
            return tracks;

        }


    }
}
