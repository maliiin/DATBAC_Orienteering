using AutoMapper;
using MediatR;
using Microsoft.EntityFrameworkCore;
using orienteering_backend.Core.Domain.Track.Dto;
using orienteering_backend.Infrastructure.Data;

namespace orienteering_backend.Core.Domain.Navigation.Pipelines
{
    public class CreateNavigationImage
    {

        public record Request(
            string path, Guid checkpointId) : IRequest<bool>;


        //fix ikke i bruk?

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
                var navigation=await _db.Navigation.Where(n => n.ToCheckpoint == request.checkpointId).FirstOrDefaultAsync(cancellationToken);
                if (navigation == null) { return false; }

                //fix order virker ikke
                var navImage = new NavigationImage(request.path, navigation.NumImages+1);
                
                navigation.AddNavigationImage(navImage);
                await _db.SaveChangesAsync(cancellationToken);

                return true;

            }
        }

    }
}
