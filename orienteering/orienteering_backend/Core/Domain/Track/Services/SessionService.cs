using Microsoft.AspNetCore.Identity;
using Microsoft.Extensions.Options;
using orienteering_backend.Core.Domain.Checkpoint.Pipelines;
using orienteering_backend.Core.Domain.Track.Dto;
using System.Web;
using MediatR;

// Lisens MediatR: https://github.com/jbogard/MediatR/blob/master/LICENSE

namespace orienteering_backend.Core.Domain.Track.Services;

//Fix: Flytte til userdomain?
public class SessionService : ISessionService
{
    private readonly IHttpContextAccessor _httpContextAccessor;
    private readonly IMediator _mediator;
    public SessionService(
        IHttpContextAccessor httpContextAccessor,
        IMediator mediator
        )
    {
        _httpContextAccessor = httpContextAccessor;
        _mediator = mediator;
    }

    public void SetStartCheckpoint(string CheckpointId)
    {
        if (_httpContextAccessor.HttpContext == null)
        {
            throw new ArgumentNullException(nameof(HttpContext));
        } 
        if (_httpContextAccessor.HttpContext.Session.GetString("StartCheckpoint") == null)
        {
            _httpContextAccessor.HttpContext.Session.SetString("StartCheckpoint", CheckpointId);
            _httpContextAccessor.HttpContext.Session.SetString("StartTime", DateTime.Now.ToString());
        }

    }

    public async Task<TrackLoggingDto> CheckTrackFinished(string CurrentCheckpoint)
    {
        if (_httpContextAccessor.HttpContext == null)
        {
            throw new Exception("Cannot access session");
        }
        var startCheckpoint = _httpContextAccessor.HttpContext.Session.GetString("StartCheckpoint");
        if (startCheckpoint == null)
        {
            throw new Exception("SessionVariable not set");
        }
        else
        {
            var toCheckpointGuid = await _mediator.Send(new GetNextCheckpoint.Request(new Guid(CurrentCheckpoint)));
            var toCheckpoint = toCheckpointGuid.ToString();
            if (startCheckpoint == toCheckpoint)
            {
                var trackLoggingDto = new TrackLoggingDto();
                trackLoggingDto.StartCheckpointId = new Guid(startCheckpoint);

                var startTimeString = _httpContextAccessor.HttpContext.Session.GetString("StartTime");
                if (startTimeString == null)
                {
                    throw new Exception("Startime not set");
                }
                var startTime = DateTime.Parse(startTimeString);
                var timeNow = DateTime.Now;
                var timeUsed = Math.Floor(timeNow.Subtract(startTime).TotalMinutes).ToString();
                trackLoggingDto.TimeUsed = timeUsed;
                _httpContextAccessor.HttpContext.Session.Remove("StartCheckpoint");
                _httpContextAccessor.HttpContext.Session.Remove("StartTime");
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

