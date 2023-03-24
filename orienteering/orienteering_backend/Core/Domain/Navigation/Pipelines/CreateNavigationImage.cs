using AutoMapper;
using MediatR;
using Microsoft.EntityFrameworkCore;
using orienteering_backend.Core.Domain.Checkpoint;
using orienteering_backend.Core.Domain.Track.Dto;
using orienteering_backend.Infrastructure.Data;

namespace orienteering_backend.Core.Domain.Navigation.Pipelines
{
    public class CreateNavigationImage
    {

        public record Request(Guid checkpointId, IFormFile file, string textDescription) : IRequest<bool>;

        public class Handler : IRequestHandler<Request, bool>
        {
            private readonly OrienteeringContext _db;

            private readonly IMapper _mapper;

            public Handler(OrienteeringContext db, IMapper mapper)
            {
                _db = db ?? throw new ArgumentNullException(nameof(db));
                _mapper = mapper ?? throw new ArgumentNullException(nameof(_mapper));

            }

            public async Task<bool> Handle(Request request, CancellationToken cancellationToken)
            {
                //get navigation
                var navigation = await _db.Navigation
                    .Where(n => n.ToCheckpoint == request.checkpointId)
                    .Include(n => n.Images)
                    .FirstOrDefaultAsync(cancellationToken);

                //fix er dette rett eller bør den lage hvis den ikke finnes?
                //den skal finnes fra før
                if (navigation == null) { return false; }

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
                //fix-ikke bruk filnavn-kan være duplisert!!
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
