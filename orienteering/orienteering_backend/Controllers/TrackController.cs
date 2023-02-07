using MediatR;
using Microsoft.AspNetCore.Mvc;
using orienteering_backend.Core.Domain.Track.Pipelines;
using orienteering_backend.Core.Domain.Track;

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
        //greate new track
        public async Task<Guid> Get()
        {
            //Fiks fakeGuid-->userGuid
            var fakeGuid = Guid.NewGuid();
            var newTrackId = await _mediator.Send(new CreateTrack.Request(fakeGuid));
            return newTrackId;

        }

        //create checkpoint
        //bør ikke dette være post??
        [HttpGet("createcheckpoint")]
        public async Task<int> CreateCheckpoint(Guid TrackId)
        {
            var newCheckPointId = await _mediator.Send(new CreateCheckpoint.Request(TrackId));
            return newCheckPointId;
        }

        //list of all tracks of a user
        [HttpGet("gettracks")]
        public async Task<List<Track>> GetTracksByUserId(Guid UserId)
        {
            var tracks = await _mediator.Send(new GetTrack.Request(UserId));
            return tracks;

        }


    }
}
