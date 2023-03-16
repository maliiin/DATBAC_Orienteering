using AutoMapper;
using MediatR;
using Microsoft.EntityFrameworkCore;
using orienteering_backend.Core.Domain.Navigation.Dto;
using orienteering_backend.Core.Domain.Track.Dto;
using orienteering_backend.Infrastructure.Data;
using SixLabors.ImageSharp;

namespace orienteering_backend.Core.Domain.Navigation.Pipelines
{
    public class GetNavigation
    {

        public record Request(
            Guid checkpointId) : IRequest<Image>;


        //fix ikke i bruk?

        public class Handler : IRequestHandler<Request, Image>
        {
            private readonly OrienteeringContext _db;

            private readonly IMapper _mapper;

            public Handler(OrienteeringContext db, IMapper mapper)
            {
                _db = db ?? throw new ArgumentNullException(nameof(db));
                _mapper = mapper ?? throw new ArgumentNullException(nameof(_mapper));

            }

            public async Task<Image> Handle(Request request, CancellationToken cancellationToken)
            {

                var navigation = await _db.Navigation
                    .Where(n => n.ToCheckpoint == request.checkpointId)
                    .Include(n=>n.Images)
                    .FirstOrDefaultAsync(cancellationToken);

                //if (navigation == null) { return false; }

                //fix dto
                //NavigationDto navDto = new();

                var testPath = navigation.Images[0].ImagePath;
                Console.WriteLine(File.ReadAllBytesAsync(testPath, cancellationToken));
                var img=Image.Load(testPath);
                return img;
                //return navigation;

         

            }
        }

    }
}
