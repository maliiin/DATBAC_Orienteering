using AutoMapper;
using MediatR;
using Microsoft.EntityFrameworkCore;
using orienteering_backend.Core.Domain.Checkpoint;
using orienteering_backend.Core.Domain.Track.Dto;
using orienteering_backend.Infrastructure.Data;

namespace orienteering_backend.Core.Domain.Navigation.Pipelines
{
    public class NavigationDeleteImage
    {

        public record Request(Guid imageId, Guid navigationId) : IRequest<bool>;

        public class Handler : IRequestHandler<Request, bool>
        {
            private readonly OrienteeringContext _db;

            private readonly IMapper _mapper;

            public Handler(OrienteeringContext db, IMapper mapper)
            {
                _db = db ?? throw new ArgumentNullException(nameof(db));
                _mapper = mapper ?? throw new ArgumentNullException(nameof(_mapper));

            }

            //delete one image from navigation
            public async Task<bool> Handle(Request request, CancellationToken cancellationToken)
            {

                var navigation = await _db.Navigation
                    .Where(n => n.Id == request.navigationId)
                    .Include(n => n.Images)
                    .FirstOrDefaultAsync(cancellationToken);

                if (navigation == null) { return false; }

                var navImage = navigation.Images.FirstOrDefault(i => i.Id == request.imageId);
                if (navImage == null) { return false; }
                var res = navigation.RemoveNavigationImage(navImage);
                if (res == false) { return false; }
                await _db.SaveChangesAsync(cancellationToken);

                //delete file
                string filePath = navImage.ImagePath;//Path.Combine(Directory.GetCurrentDirectory(), "wwwroot", navigation.ToCheckpoint.ToString(), navImage. );
                File.Delete(filePath);

                return true;

            }
        }

    }
}
