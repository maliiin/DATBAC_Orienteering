using AutoMapper;
using MediatR;
using Microsoft.EntityFrameworkCore;
using orienteering_backend.Core.Domain.Navigation.Dto;
using orienteering_backend.Core.Domain.Track.Dto;
using orienteering_backend.Infrastructure.Data;
using SixLabors.ImageSharp;
using System.Collections.Generic;

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

            public Handler(OrienteeringContext db, IMapper mapper)
            {
                _db = db ?? throw new ArgumentNullException(nameof(db));
                _mapper = mapper ?? throw new ArgumentNullException(nameof(_mapper));

            }

            public async Task<NavigationDto> Handle(Request request, CancellationToken cancellationToken)
            {
                //get nav from db
                Navigation? navigation = await _db.Navigation
                    .Where(n => n.ToCheckpoint == request.checkpointId)
                    .Include(n => n.Images)
                    .FirstOrDefaultAsync(cancellationToken);

                if (navigation == null) { throw new NullReferenceException("Navigation is null"); }

                List<NavigationImageDto> imgDtoList = new();
                foreach (NavigationImage navImage in navigation.Images)
                {
                    byte[] imgByte;
                    string fileType;

                    string path = navImage.ImagePath;
                    using (FileStream t = System.IO.File.Open(path, FileMode.Open, FileAccess.Read, FileShare.Read))
                    {
                        imgByte = System.IO.File.ReadAllBytes(path);
                        fileType = System.IO.Path.GetExtension(path).Remove(0,1);
                        
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
