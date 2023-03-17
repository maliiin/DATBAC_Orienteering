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


        //fix ikke i bruk?

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

                //fix dto
                //NavigationDto navDto = new();
                List<NavigationImageDto> imgDtoList = new();
                foreach (NavigationImage navImage in navigation.Images)
                {
                    byte[] imgByte;

                    string path = navImage.ImagePath;
                    using (FileStream t = System.IO.File.Open(path, FileMode.Open, FileAccess.Read, FileShare.Read))
                    {
                        imgByte = System.IO.File.ReadAllBytes(path);

                    }

                    //convert to dto
                    NavigationImageDto imgDto = _mapper.Map<NavigationImage, NavigationImageDto>(navImage);
                    imgDto.ImageData = imgByte;
                    imgDtoList.Add(imgDto);

                }

                var navDto = _mapper.Map<Navigation, NavigationDto>(navigation);
                navDto.Images= imgDtoList;
                return navDto;


                //string path = Path.Combine(Directory.GetCurrentDirectory(), "wwwroot", "08db26c5-298c-4398-847c-7d9ad2136e02", "a bilde.png");
                //byte[] ret;

                ////fix usikker på om fileshare
                //using (FileStream t = System.IO.File.Open(path, FileMode.Open, FileAccess.Read, FileShare.Read))
                //{
                //    ret = System.IO.File.ReadAllBytes(path);

                //}

                //string testPath = navigation.Images[0].ImagePath;
                //Image img;
                //using (FileStream stream = System.IO.File.Open(testPath, FileMode.Open))
                //{
                //    Console.WriteLine(File.ReadAllBytesAsync(testPath, cancellationToken));
                //    img = Image.Load(testPath);

                //}

                //return img;
                ////return navigation;



            }
        }

    }
}
