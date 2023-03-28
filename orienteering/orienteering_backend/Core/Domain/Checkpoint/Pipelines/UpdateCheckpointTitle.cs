using MediatR;
using Microsoft.EntityFrameworkCore;
using orienteering_backend.Core.Domain.Checkpoint.Dto;
using orienteering_backend.Core.Domain.Checkpoint;
using orienteering_backend.Infrastructure.Data;
using orienteering_backend.Core.Domain.Checkpoint.Events;
using System.Security.Authentication;
using orienteering_backend.Core.Domain.Track.Dto;
using orienteering_backend.Core.Domain.Track.Pipelines;
using AutoMapper;
using orienteering_backend.Core.Domain.Authentication.Services;
//Kilder: CampusEats lab fra dat240
// Kilder: https://github.com/dat240-2022/assignments/blob/main/Lab3/UiS.Dat240.Lab3/Core/Domain/Cart/Pipelines/AddItem.cs (07.02.2023)
// Brukte samme struktur på pipelinen som i kilden

namespace orienteering_backend.Core.Domain.Checkpoint.Pipelines;

public static class UpdateCheckpointTitle
{
    public record Request(
        string checkpointTitle, Guid checkpointId) : IRequest<Checkpoint>;


    public class Handler : IRequestHandler<Request, Checkpoint>
    {
        private readonly OrienteeringContext _db;
        private readonly IMapper _mapper;
        private readonly IIdentityService _identityService;
        private readonly IMediator _mediator;


        public Handler(OrienteeringContext db, IMapper mapper, IIdentityService identityService, IMediator mediator)
        {
            _db = db ?? throw new ArgumentNullException(nameof(db));
            _mapper = mapper;
            _identityService = identityService;
            _mediator = mediator;
        }


        public async Task<Checkpoint?> Handle(Request request, CancellationToken cancellationToken)
        {
            //check that signed in
            var userId = _identityService.GetCurrentUserId();
            if (userId == null) { throw new AuthenticationException("user not signed in"); }

            //get checkpoint
            var checkpoint = await _db.Checkpoints
                .Where(ch => ch.Id == request.checkpointId)
                .FirstOrDefaultAsync(cancellationToken);

            if (checkpoint == null) { return null; };

            //check that user is allowed to access this checkpoint
            TrackUserIdDto track = await _mediator.Send(new GetTrackUser.Request(checkpoint.TrackId));
            if (userId != track.UserId) { throw new AuthenticationException(); }

            //change title
            checkpoint.Title = request.checkpointTitle;
            await _db.SaveChangesAsync(cancellationToken);
            return checkpoint;
        }
    }

}
