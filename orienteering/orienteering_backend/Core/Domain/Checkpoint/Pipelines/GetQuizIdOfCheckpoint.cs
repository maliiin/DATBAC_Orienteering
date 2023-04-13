﻿using MediatR;
using Microsoft.EntityFrameworkCore;
using orienteering_backend.Infrastructure.Data;
namespace orienteering_backend.Core.Domain.Checkpoint.Pipelines;

//unauthorized way to get quizId of checkpoint
public static class GetQuizIdOfCheckpoint
{
    public record Request(
        Guid checkpointId) : IRequest<Guid?>;
    public class Handler : IRequestHandler<Request, Guid?>
    {
        private readonly OrienteeringContext _db;

        public Handler(OrienteeringContext db)
        {
            _db = db ?? throw new ArgumentNullException(nameof(db));
        }

        public async Task<Guid?> Handle(Request request, CancellationToken cancellationToken)
        {
            var checkpoint = await _db.Checkpoints
                .Where(c => c.Id == request.checkpointId)
                .FirstOrDefaultAsync(cancellationToken);

            if (checkpoint is null) { throw new NullReferenceException("the checkpoint cannot be found or not allowed to access"); };
            return checkpoint.QuizId;
        }
    }

}
