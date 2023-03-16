using AutoMapper;
using MediatR;
using orienteering_backend.Core.Domain.Track.Dto;
using orienteering_backend.Infrastructure.Data;

namespace orienteering_backend.Core.Domain.Navigation.Pipelines
{
    public class CreateNavigation
    {

        public record Request(
            string path, Guid checkpointId) : IRequest<Guid>;


        //fix ikke i bruk?

        public class Handler : IRequestHandler<Request, Guid>
        {
            private readonly OrienteeringContext _db;

            private readonly IMapper _mapper;

            public Handler(OrienteeringContext db, IMapper mapper)
            {
                _db = db ?? throw new ArgumentNullException(nameof(db));
                _mapper = mapper ?? throw new ArgumentNullException(nameof(_mapper));

            }

            public async Task<Guid> Handle(Request request, CancellationToken cancellationToken)
            {
                var navigation = new Navigation();
                navigation.ToCheckpoint = request.checkpointId;

                await _db.Navigation.AddAsync(navigation, cancellationToken);
                await _db.SaveChangesAsync(cancellationToken);
                return navigation.Id;

            }
        }

    }
}
