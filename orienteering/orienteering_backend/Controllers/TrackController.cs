using MediatR;
using Microsoft.AspNetCore.Mvc;
using orienteering_backend.Core.Domain.Track.Pipelines;
using orienteering_backend.Core.Domain.Authentication;
using Microsoft.AspNetCore.Identity;
using orienteering_backend.Core.Domain.Track.Dto;
using orienteering_backend.Core.Domain.Checkpoint.Dto;
using orienteering_backend.Core.Domain.Checkpoint.Pipelines;
using orienteering_backend.Core.Domain.Checkpoint;
using Microsoft.AspNetCore.Authorization;

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
        //bør userGuid sendes inn fra frontend? eller skal backend hente userId fra seg selv fra den som er logget inn?

        //greate new track
        //POST
       // [Authorize]
        [HttpPost("createTrack")]
        public async Task<Guid> CreateTrack(TrackDto trackDto)
        {
            var newTrackId = await _mediator.Send(new CreateTrack.Request(trackDto));
            return newTrackId;
        }

        
        //list of all tracks of a user
        [HttpGet("getTracks")]
        public async Task<List<TrackDto>> GetTracksByUserId(string userId)
        {
            var UserId =  new Guid(userId);
            var tracks = await _mediator.Send(new GetTracks.Request(UserId));
            return tracks;
        }


        [HttpGet("getTrack")]
        public async Task<TrackDto> GetSingleTrack(string trackId)
        {

            Guid trackGuid = new Guid(trackId);
            TrackDto trackDto = await _mediator.Send(new GetSingleTrack.Request(trackGuid));

            return trackDto;

        }

    }

}
