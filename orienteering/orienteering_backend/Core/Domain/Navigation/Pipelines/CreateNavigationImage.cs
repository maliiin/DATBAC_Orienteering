using MediatR;
using Microsoft.EntityFrameworkCore;
using orienteering_backend.Core.Domain.Authentication.Services;
using orienteering_backend.Core.Domain.Checkpoint.Dto;
using orienteering_backend.Core.Domain.Checkpoint.Pipelines;
using orienteering_backend.Core.Domain.Track.Dto;
using orienteering_backend.Core.Domain.Track.Pipelines;
using orienteering_backend.Infrastructure.Data;
using System.Security.Authentication;

// License MediatR (Apache 2.0): https://github.com/jbogard/MediatR/blob/master/LICENSE

namespace orienteering_backend.Core.Domain.Navigation.Pipelines
{
    public class CreateNavigationImage
    {

        public record Request(Guid checkpointId, IFormFile file, string textDescription) : IRequest<bool>;

        public class Handler : IRequestHandler<Request, bool>
        {
            private readonly OrienteeringContext _db;
            private readonly IIdentityService _identityService;
            private readonly IMediator _mediator;

            public Handler(OrienteeringContext db,IIdentityService identityService, IMediator mediator)
            {
                _db = db ?? throw new ArgumentNullException(nameof(db));
                _identityService = identityService;
                _mediator = mediator;
            }

            public async Task<bool> Handle(Request request, CancellationToken cancellationToken)
            {
                //check that signed in
                var userId = _identityService.GetCurrentUserId();
                if (userId == null) { throw new AuthenticationException("user not signed in"); }

                //check that user is allowed to access this navigation
                CheckpointDto checkpoint = await _mediator.Send(new GetSingleCheckpoint.Request(request.checkpointId));
                TrackUserIdDto track = await _mediator.Send(new GetTrackUser.Request(checkpoint.TrackId));
                if (userId != track.UserId) { throw new ArgumentNullException("not found or access not allowed"); }

                //get navigation
                var navigation = await _db.Navigation
                    .Where(n => n.ToCheckpoint == request.checkpointId)
                    .Include(n => n.Images)
                    .FirstOrDefaultAsync(cancellationToken);

                if (navigation == null) { throw new ArgumentNullException("not found or access not allowed"); }

                //wwwroot/checkpointId is the folder
                string folder = $"{request.checkpointId}";
                string dirPath = Path.Combine(Directory.GetCurrentDirectory(), "wwwroot", folder);

                //create dir if not exists
                if (!Directory.Exists(dirPath))
                {
                    Directory.CreateDirectory(dirPath);
                }

                //this is the filename->dont have duplicate filenames
                var randomGuid = Guid.NewGuid();


                //place image in correct place in file system
                string path = Path.Combine(
                    Directory.GetCurrentDirectory(),
                    "wwwroot",
                    folder,
                    randomGuid.ToString() + Path.GetExtension(request.file.FileName)
                );

                using (var memoryStream = new FileStream(path, FileMode.Create))
                {
                    request.file.CopyTo(memoryStream);
                }

                var navImage = new NavigationImage(path, navigation.NumImages + 1, request.textDescription);
                navigation.AddNavigationImage(navImage);
                await _db.SaveChangesAsync(cancellationToken);
                return true;
            }
        }
    }
}
