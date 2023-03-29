﻿using MediatR;
using Microsoft.EntityFrameworkCore;
using orienteering_backend.Infrastructure.Data;
using orienteering_backend.Core.Domain.Quiz.Dto;
using AutoMapper;
using System.Security.Authentication;
using orienteering_backend.Core.Domain.Authentication.Services;
using orienteering_backend.Core.Domain.Track.Pipelines;
using Microsoft.AspNetCore.Server.IIS.Core;
//Kilder: CampusEats lab fra dat240
// Kilder: https://github.com/dat240-2022/assignments/blob/main/Lab3/UiS.Dat240.Lab3/Core/Domain/Cart/Pipelines/AddItem.cs (07.02.2023)
// Brukte samme struktur på pipelinen som i kilden

namespace orienteering_backend.Core.Domain.Quiz.Pipelines;

public static class GetQuiz
{
    public record Request(
        Guid QuizId
        ) : IRequest<QuizDto>;


    public class Handler : IRequestHandler<Request, QuizDto>
    {
        private readonly OrienteeringContext _db;
        private readonly IMapper _mapper;
        private readonly IIdentityService _identityService;
        private readonly IMediator _mediator;

        public Handler(OrienteeringContext db, IMapper mapper, IIdentityService identityService, IMediator mediator)
        {
            _db = db ?? throw new ArgumentNullException(nameof(db));
            _mapper = mapper;
            _identityService = identityService;
            _mediator = mediator;
        }
        public async Task<QuizDto> Handle(Request request, CancellationToken cancellationToken)
        {
            //check that signed in
            var userId = _identityService.GetCurrentUserId();
            if (userId == null) { throw new AuthenticationException("user not signed in"); }

            //check that user is allowed to access this quiz
            var trackUser = await _mediator.Send(new GetTrackUserByQuiz.Request(request.QuizId));
            if (trackUser.UserId != userId)
            {
                //the user that owns the track is not the one signed in
                throw new AuthenticationException("the quiz is not owned by that user");
            }

            //get quiz
            var quiz = await _db.Quiz
                .Include(q => q.QuizQuestions)
                .ThenInclude(a => a.Alternatives)
                .FirstOrDefaultAsync(q => q.Id == request.QuizId, cancellationToken);

            if (quiz == null) { throw new Exception("Quiz not found"); }

            //var quizDto = _mapper.Map<Quiz, QuizDto>(quiz);
            var dtoList = new List<QuizQuestionDto>();
            for (var i = 0; i < quiz.QuizQuestions.Count; i++)
            {
                var quizQuestion = quiz.QuizQuestions[i];
                //var dtoElement = new QuizQuestionDto(quizQuestion.Question, quizQuestion.CorrectAlternative);
                //dtoElement.Alternatives = quizQuestion.Alternatives;
                //dtoElement.QuizQuestionId = quizQuestion.Id;
                var quizQuestionDto = _mapper.Map<QuizQuestion, QuizQuestionDto>(quizQuestion);
                dtoList.Add(quizQuestionDto);
            }
            //var quizDto = _mapper.Map<Quiz, QuizDto>(Quiz);
            var quizDto = new QuizDto(quiz.Id, dtoList);
            return quizDto;
        }
    }

}
