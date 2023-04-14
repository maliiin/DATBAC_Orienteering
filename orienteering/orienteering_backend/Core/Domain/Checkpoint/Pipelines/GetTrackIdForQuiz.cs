using MediatR;
using Microsoft.EntityFrameworkCore;
using orienteering_backend.Infrastructure.Data;
using orienteering_backend.Core.Domain.Checkpoint.Dto;
using AutoMapper;
using orienteering_backend.Core.Domain.Authentication.Services;
using System.Security.Authentication;
using orienteering_backend.Core.Domain.Track.Dto;
using orienteering_backend.Core.Domain.Track.Pipelines;

namespace orienteering_backend.Core.Domain.Checkpoint.Pipelines;

public static class GetTrackIdForQuiz
{
    public record Request(
        Guid quizId) : IRequest<Guid>;


    public class Handler : IRequestHandler<Request, Guid>
    {
        private readonly OrienteeringContext _db;
        

        public Handler(OrienteeringContext db)
        {
            _db = db ?? throw new ArgumentNullException(nameof(db));
            
        }

        //denne er ikke autentisert fordi den kun kalles internt
        //return id of checkpoint qith spesific quizId

        public async Task<Guid> Handle(Request request, CancellationToken cancellationToken)
        {
            var checkpoint = await _db.Checkpoints
                .Where(c => c.QuizId == request.quizId)
                .FirstOrDefaultAsync(cancellationToken);

            if (checkpoint is null) { throw new NullReferenceException("the checkpoint cannot be found"); };
            return checkpoint.TrackId;
        }
    }

}

