using Microsoft.AspNetCore.Mvc;
using MediatR;
using orienteering_backend.Core.Domain.Track.Dto;

namespace orienteering_backend.Controllers;

[ApiController]
[Route("api/session")]
public class SessionController : ControllerBase
{
    private readonly IMediator _mediator;
    public SessionController(IMediator Mediator)
    {
        _mediator = Mediator;
    }

    [HttpGet("setStartCheckpoint")]
    public string setStartCheckpoint(string CheckpointId)
    {
        if (HttpContext.Session.GetString("StartCheckpoint") == null)
        {
            HttpContext.Session.SetString("StartCheckpoint", CheckpointId);
            HttpContext.Session.SetString("StartTime", DateTime.Now.ToString());
        }

        return CheckpointId;
    }

    [HttpGet("checkTrackFinished")]
    public async Task<TrackLoggingDto> checkTrackFinished(string currentCheckpoint)
    {
        if (HttpContext.Session.GetString("StartCheckpoint") == null)
        {
            throw new Exception("SessionVariable not set");
        }
        else
        {
            var startCheckpoint = HttpContext.Session.GetString("StartCheckpoint");
            var toCheckpointGuid = await _mediator.Send(new GetNextCheckpoint.Request(new Guid(currentCheckpoint)));
            var toCheckpoint = toCheckpointGuid.ToString();
            if (startCheckpoint == toCheckpoint)
            {
                var trackLoggingDto = new TrackLoggingDto();
                trackLoggingDto.StartCheckpointId = new Guid(startCheckpoint);

                var startTimeString = HttpContext.Session.GetString("StartTime");
                if (startTimeString == null)
                {
                    throw new Exception("Startime not set");
                }
                var startTime = DateTime.Parse(startTimeString);
                var timeNow = DateTime.Now;
                var timeUsed = Math.Floor(timeNow.Subtract(startTime).TotalMinutes).ToString();
                trackLoggingDto.TimeUsed = timeUsed;
                HttpContext.Session.Remove("StartCheckpoint");
                HttpContext.Session.Remove("StartTime");
                return trackLoggingDto;
            }
            else
            {
                var trackLoggingDto = new TrackLoggingDto();
                trackLoggingDto.StartCheckpointId = new Guid(startCheckpoint);
                return trackLoggingDto;
            }
        }
    }
}