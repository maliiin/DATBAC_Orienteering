using AutoMapper;
using MediatR;
using Microsoft.EntityFrameworkCore;
using orienteering_backend.Core.Domain.Authentication.Services;
using orienteering_backend.Core.Domain.Navigation.Dto;
using orienteering_backend.Infrastructure.Data;

// Lisens MediatR: https://github.com/jbogard/MediatR/blob/master/LICENSE

namespace orienteering_backend.Core.Domain.Navigation.Pipelines
{
    public class GetNavigationUnauthorized
    {
        public record Request(
            Guid checkpointId) : IRequest<NavigationDto>;

        public class Handler : IRequestHandler<Request, NavigationDto>
        {
            private readonly OrienteeringContext _db;
            private readonly IMapper _mapper;


            public Handler(OrienteeringContext db, IMapper mapper)
            {
                _db = db ?? throw new ArgumentNullException(nameof(db));
                _mapper = mapper;
                
            }

            public async Task<NavigationDto> Handle(Request request, CancellationToken cancellationToken)
            {
                

                //get nav from db
                Navigation? navigation = await _db.Navigation
                    .Where(n => n.ToCheckpoint == request.checkpointId)
                    .Include(n => n.Images)
                    .FirstOrDefaultAsync(cancellationToken);

                if (navigation == null) { throw new NullReferenceException("Navigation is null or access not allowed"); }

                List<NavigationImageDto> imgDtoList = new();
                foreach (NavigationImage navImage in navigation.Images)
                {
                    byte[] imgByte;
                    string fileType;

                    string path = navImage.ImagePath;
                    using (FileStream t = File.Open(path, FileMode.Open, FileAccess.Read, FileShare.Read))
                    {
                        imgByte = File.ReadAllBytes(path);
                        fileType = Path.GetExtension(path).Remove(0, 1);
                    }

                    //convert to dto
                    NavigationImageDto imgDto = _mapper.Map<NavigationImage, NavigationImageDto>(navImage);
                    imgDto.ImageData = imgByte;
                    imgDto.fileType = fileType;
                    imgDtoList.Add(imgDto);
                }

                var navDto = _mapper.Map<Navigation, NavigationDto>(navigation);
                navDto.Images = imgDtoList;
                return navDto;
            }
        }
    }
}
