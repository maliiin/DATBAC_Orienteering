using AutoMapper;
using MediatR;
using Microsoft.EntityFrameworkCore;
using orienteering_backend.Core.Domain.Checkpoint.Pipelines;
using orienteering_backend.Core.Domain.Track.Dto;
using orienteering_backend.Infrastructure.Data;

// Lisens MediatR: https://github.com/jbogard/MediatR/blob/master/LICENSE

namespace orienteering_backend.Core.Domain.Track.Pipelines;

public static class GetTrackUserByQuiz
{
    public record Request(
        Guid quizId) : IRequest<TrackUserIdDto>;


    public class Handler : IRequestHandler<Request, TrackUserIdDto>
    {
        private readonly OrienteeringContext _db;
        private readonly IMapper _mapper;
        private readonly IMediator _mediator;

        public Handler(OrienteeringContext db, IMapper mapper,  IMediator mediator)
        {
            _db = db ?? throw new ArgumentNullException(nameof(db));
            _mapper = mapper;
            _mediator = mediator;
        }

        public async Task<TrackUserIdDto> Handle(Request request, CancellationToken cancellationToken)
        { 
            //get trackid from checkpoint domain
            var trackId = await _mediator.Send(new GetTrackIdForQuiz.Request(request.quizId));

            //get track
            var track = await _db.Tracks
                .Where(t => t.Id == trackId)
                .FirstOrDefaultAsync(cancellationToken);

            if (track == null) { throw new NullReferenceException(); }

            TrackUserIdDto trackDto = _mapper.Map<TrackUserIdDto>(track);
            return trackDto;
        }
    }
}
