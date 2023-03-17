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

        public record Request(Guid checkpointId, IFormFile file) : IRequest<bool>;

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
                string folder = $"{request.checkpointId}";

                //wwwroot/checkpointId is the path
                string dirPath = Path.Combine(Directory.GetCurrentDirectory(), "wwwroot", folder);

                //create wwwroot/checkpointId dir if not exists
                if (!Directory.Exists(dirPath))
                {
                    Directory.CreateDirectory(dirPath);
                }

                //place image in correct place in file system
                //fix-ikke bruk filnavn-kan være duplisert!!
                string path = Path.Combine(Directory.GetCurrentDirectory(), "wwwroot", folder, request.file.FileName);
                using (var memoryStream = new FileStream(path, FileMode.Create))
                {
                    request.file.CopyTo(memoryStream);
                }


                var navigation=await _db.Navigation
                    .Where(n => n.ToCheckpoint == request.checkpointId)
                    .Include(n=>n.Images)
                    .FirstOrDefaultAsync(cancellationToken);

                if (navigation == null) { return false; }

               
                //fix order virker ikke
                var navImage = new NavigationImage(path, navigation.NumImages+1);
                
                navigation.AddNavigationImage(navImage);
                await _db.SaveChangesAsync(cancellationToken);

                return true;

            }
        }

    }
}
