using Microsoft.AspNetCore.Mvc;
using System.Security.Authentication;
using MediatR;
using orienteering_backend.Core.Domain.Checkpoint.Dto;
using orienteering_backend.Core.Domain.Checkpoint.Pipelines;

namespace orienteering_backend.Controllers;

//fix-fjern sixlabors imagesharp fra pakker!!
[ApiController]
[Route("api/qrcode")]
public class QRCodeController : ControllerBase
{
    private readonly IMediator _mediator;
    public QRCodeController(IMediator Mediator)
    {
        _mediator = Mediator;
    }
    
    [HttpGet("getqrcodes")]
    public async Task<ActionResult<List<CheckpointNameAndQRCodeDto>>> GetQRCodes(string TrackId)
    {
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