using Microsoft.AspNetCore.Mvc;
using MediatR;
using orienteering_backend.Core.Domain.Track.Dto;
using orienteering_backend.Core.Domain.Checkpoint.Pipelines;
using orienteering_backend.Core.Domain.Quiz.Pipelines;
using orienteering_backend.Core.Domain.Track.Pipelines;
using orienteering_backend.Core.Domain.Track.Services;

// Lisens MediatR: https://github.com/jbogard/MediatR/blob/master/LICENSE

namespace orienteering_backend.Controllers;

[ApiController]
[Route("api/session")]
public class SessionController : ControllerBase
{
    private readonly ISessionService _sessionService;
    public SessionController(ISessionService sessionService)
    {
        _sessionService = sessionService;
    }
    // fix: post eller get?
    [HttpPost("setStartCheckpoint")]
    public void setStartCheckpoint(SessionDto CheckpointInfo)
    {
        _sessionService.SetStartCheckpoint(CheckpointInfo.CheckpointId.ToString());
    }

    [HttpGet("checkTrackFinished")]
    //fix-flytt til pipeline kanskje!!
    public async Task<TrackLoggingDto> checkTrackFinished(string CurrentCheckpoint)
    {
        var trackLoggingDto = await _sessionService.CheckTrackFinished(CurrentCheckpoint);
        return trackLoggingDto;
    }
}