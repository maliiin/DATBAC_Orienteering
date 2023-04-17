﻿using Microsoft.AspNetCore.Mvc;
using System.Security.Authentication;
using MediatR;
using orienteering_backend.Core.Domain.Checkpoint.Dto;
using orienteering_backend.Core.Domain.Checkpoint.Pipelines;

// Lisens MediatR: https://github.com/jbogard/MediatR/blob/master/LICENSE

namespace orienteering_backend.Controllers;

[ApiController]
[Route("api/qrcode")]
public class QRCodeController : ControllerBase
{
    private readonly IMediator _mediator;
    public QRCodeController(IMediator Mediator)
    {
        _mediator = Mediator;
    }
    // fix flytt til checkpointcontroller og fjern qrcodecontroller
    [HttpGet("getqrcodes")]
    public async Task<ActionResult<List<CheckpointNameAndQRCodeDto>>> GetQRCodes(string TrackId)
    {
        if (!ModelState.IsValid) { return BadRequest(ModelState); }

        var track = new Guid(TrackId);

        try
        {
            var checkpointList = await _mediator.Send(new GetQRCodes.Request(track));
            return checkpointList;
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
}