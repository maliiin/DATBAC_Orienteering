using AutoMapper;
using MediatR;
using orienteering_backend.Core.Domain.Authentication.Services;
using orienteering_backend.Core.Domain.Checkpoint.Pipelines;
using orienteering_backend.Core.Domain.Track.Dto;
using orienteering_backend.Infrastructure.Data;
using System.Security.Authentication;

// Lisens MediatR: https://github.com/jbogard/MediatR/blob/master/LICENSE

namespace orienteering_backend.Core.Domain.Track.Pipelines;

public static class CheckTrackFinished
{
    public record Request(
        string currentCheckpoint) : IRequest<TrackLoggingDto>;


    public class Handler : IRequestHandler<Request, TrackLoggingDto>
    {
        private readonly IHttpContextAccessor _httpContextAccessor;
        private readonly IMediator _mediator;


        public Handler(OrienteeringContext db, IHttpContextAccessor httpContextAccessor, IMediator mediator)
        {
            //_db = db ?? throw new ArgumentNullException(nameof(db));
            _httpContextAccessor = httpContextAccessor;
            _mediator = mediator;
    }

        public async Task<TrackLoggingDto> Handle(Request request, CancellationToken cancellationToken)
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
                var toCheckpointGuid = await _mediator.Send(new GetNextCheckpoint.Request(new Guid(request.currentCheckpoint)));
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

}
