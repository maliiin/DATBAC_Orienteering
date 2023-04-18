﻿using Microsoft.AspNetCore.Mvc;
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
    [HttpPost("setStartCheckpoint")]
    public ActionResult setStartCheckpoint(SessionDto CheckpointInfo)
    {
        if (!ModelState.IsValid) { return BadRequest(ModelState); }

        _sessionService.SetStartCheckpoint(CheckpointInfo.CheckpointId.ToString());
        return Ok();
    }

    [HttpGet("checkTrackFinished")]
    public async Task<ActionResult<TrackLoggingDto>> checkTrackFinished(string CurrentCheckpoint)
    {
        if (!ModelState.IsValid) { return BadRequest(ModelState); }

        var trackLoggingDto = await _sessionService.CheckTrackFinished(CurrentCheckpoint);
        return trackLoggingDto;
    }
}