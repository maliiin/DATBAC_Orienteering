using orienteering_backend.Core.Domain.Checkpoint.Pipelines;
using orienteering_backend.Core.Domain.Track.Dto;
using MediatR;
using orienteering_backend.Core.Domain.Checkpoint;

// License MediatR (Apache 2.0): https://github.com/jbogard/MediatR/blob/master/LICENSE

namespace orienteering_backend.Core.Domain.Track.Services;

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
            _httpContextAccessor.HttpContext.Session.SetInt32("Score", 0);
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
        var toCheckpointGuid = await _mediator.Send(new GetNextCheckpoint.Request(new Guid(CurrentCheckpoint)));
        var trackLoggingDto = new TrackLoggingDto();
        trackLoggingDto.StartCheckpointId = new Guid(startCheckpoint);

        var toCheckpoint = toCheckpointGuid.ToString();
        if (startCheckpoint == toCheckpoint)
        {

            var startTimeString = _httpContextAccessor.HttpContext.Session.GetString("StartTime");
            if (startTimeString == null)
            {
                throw new Exception("Startime not set");
            }
            var startTime = DateTime.Parse(startTimeString);
            var timeNow = DateTime.Now;
            var timeUsed = Math.Floor(timeNow.Subtract(startTime).TotalMinutes).ToString();
            var score = _httpContextAccessor.HttpContext.Session.GetInt32("Score");
            trackLoggingDto.TimeUsed = timeUsed;
            trackLoggingDto.Score = score;
            //clear session
            _httpContextAccessor.HttpContext.Session.Remove("StartCheckpoint");
            _httpContextAccessor.HttpContext.Session.Remove("StartTime");
            _httpContextAccessor.HttpContext.Session.Remove("Score");
            }

        return trackLoggingDto;
    }

    public void AddScore(int points)
    {
        if (_httpContextAccessor.HttpContext == null)
        {
            throw new ArgumentNullException(nameof(HttpContext));
        }
        var previousScore = _httpContextAccessor.HttpContext.Session.GetInt32("Score");
        if (previousScore == null) { throw new ArgumentNullException("score is null"); }

        //add score to session
        _httpContextAccessor.HttpContext.Session.SetInt32("Score",(int)previousScore+points);
    }
}

