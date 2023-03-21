using Microsoft.AspNetCore.Mvc;

using MediatR;
using orienteering_backend.Core.Domain.Track.Dto;
using orienteering_backend.Core.Domain.Checkpoint.Pipelines;
using System.Web.Helpers;

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
            Console.WriteLine(HttpContext.Session.GetString("StartCheckpoint"));
        }
       
        return CheckpointId;
    }

   
   

    [HttpGet("checkTrackFinished")]
    public TrackLoggingDto getStartCheckpoint(string toCheckpoint)
    {
        if (HttpContext.Session.GetString("StartCheckpoint") == null)
        {
            throw new Exception("SessionVariable not set");
        }
        else
        {
            var startCheckpoint = HttpContext.Session.GetString("StartCheckpoint");
            if (startCheckpoint == toCheckpoint)
            {
                var trackLoggingDto = new TrackLoggingDto();
                trackLoggingDto.StartCheckpointId = new Guid(startCheckpoint);
                var startTimeString = HttpContext.Session.GetString("StartTime");
                var startTime = DateTime.Parse(startTimeString);
                trackLoggingDto.TimeUsed = startTime;
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