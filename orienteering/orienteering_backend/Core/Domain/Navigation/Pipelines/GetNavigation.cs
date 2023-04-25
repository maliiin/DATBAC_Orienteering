using AutoMapper;
using MediatR;
using Microsoft.EntityFrameworkCore;
using orienteering_backend.Core.Domain.Authentication.Services;
using orienteering_backend.Core.Domain.Checkpoint.Dto;
using orienteering_backend.Core.Domain.Checkpoint.Pipelines;
using orienteering_backend.Core.Domain.Navigation.Dto;
using orienteering_backend.Core.Domain.Track.Dto;
using orienteering_backend.Core.Domain.Track.Pipelines;
using orienteering_backend.Infrastructure.Data;
using System.Security.Authentication;

// License MediatR (Apache 2.0): https://github.com/jbogard/MediatR/blob/master/LICENSE

namespace orienteering_backend.Core.Domain.Navigation.Pipelines
{
    public class GetNavigation
    {
        public record Request(
            Guid checkpointId) : IRequest<NavigationDto>;

        public class Handler : IRequestHandler<Request, NavigationDto>
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

            public async Task<NavigationDto> Handle(Request request, CancellationToken cancellationToken)
            {
                //check that signed in
                var userId = _identityService.GetCurrentUserId();
                if (userId == null) { throw new AuthenticationException("user not signed in"); }
 

                //get nav from db
                Navigation? navigation = await _db.Navigation
                    .Where(n => n.ToCheckpoint == request.checkpointId)
                    .Include(n => n.Images)
                    .FirstOrDefaultAsync(cancellationToken);

                if (navigation == null) { throw new ArgumentNullException("Navigation is null or access not allowed"); }

                //check that user is allowed to access this navigation
                CheckpointDto checkpoint = await _mediator.Send(new GetSingleCheckpoint.Request(navigation.ToCheckpoint));
                TrackUserIdDto track = await _mediator.Send(new GetTrackUser.Request(checkpoint.TrackId));
                if (userId != track.UserId) { throw new ArgumentNullException("Navigation is null or access not allowed"); }

                List<NavigationImageDto> imgDtoList = new();
                foreach (NavigationImage navImage in navigation.Images)
                {
                    byte[] imgByte;
                    string fileType;

                    string path = navImage.ImagePath;
                    using (FileStream t = File.Open(path, FileMode.Open, FileAccess.Read, FileShare.Read))
                    {
                        imgByte = File.ReadAllBytes(path);
                        fileType = Path.GetExtension(path).Remove(0,1);
                        
                    }

                    //convert to dto
                    NavigationImageDto imgDto = _mapper.Map<NavigationImage, NavigationImageDto>(navImage);
                    imgDto.ImageData = imgByte;
                    imgDto.fileType= fileType;
                    imgDtoList.Add(imgDto);

                }

                var navDto = _mapper.Map<Navigation, NavigationDto>(navigation);
                navDto.Images= imgDtoList;
                return navDto;

            }
        }
    }
}
