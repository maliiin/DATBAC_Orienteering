using MediatR;
using Microsoft.EntityFrameworkCore;
using orienteering_backend.Infrastructure.Data;
using orienteering_backend.Core.Domain.Quiz.Dto;
using AutoMapper;
//Kilder: CampusEats lab fra dat240
// Kilder: https://github.com/dat240-2022/assignments/blob/main/Lab3/UiS.Dat240.Lab3/Core/Domain/Cart/Pipelines/AddItem.cs (07.02.2023)
// Brukte samme struktur på pipelinen som i kilden

namespace orienteering_backend.Core.Domain.Quiz.Pipelines;

public static class GetNextQuizQuestion
{
    public record Request(
        Guid QuizId,
        int quizQuestionIndex
        ) : IRequest<NextQuizQuestionDto>;


    public class Handler : IRequestHandler<Request, NextQuizQuestionDto>
    {
        private readonly OrienteeringContext _db;
        private readonly IMapper _mapper;

        public Handler(OrienteeringContext db, IMapper mapper)
        {
            _db = db ?? throw new ArgumentNullException(nameof(db));
            _mapper = mapper ?? throw new ArgumentNullException(nameof(_mapper));

        }
        public async Task<NextQuizQuestionDto> Handle(Request request, CancellationToken cancellationToken)
        {
            var Quiz = await _db.Quiz.Include(q => q.QuizQuestions).ThenInclude(a => a.Alternatives).FirstOrDefaultAsync(q => q.Id == request.QuizId, cancellationToken);
            if (Quiz == null)
            {
                throw new Exception("Quiz not found");
            }
            if (Quiz.QuizQuestions.Count <= request.quizQuestionIndex)
            {
                return new NextQuizQuestionDto();
            }
            var quizQuestion = Quiz.QuizQuestions[request.quizQuestionIndex];
            var nextQuizQuestion = new NextQuizQuestionDto();
            var alternativeDtoList = new List<AlternativeDto>();
            for (var i=0; i<quizQuestion.Alternatives.Count; i++)
            {
                var alternativeDto = _mapper.Map<Alternative, AlternativeDto>(quizQuestion.Alternatives[i]);
                //var alternativeDto = new AlternativeDto();
                //alternativeDto.Id = quizQuestion.Alternatives[i].Id;
                //alternativeDto.Text = quizQuestion.Alternatives[i].Text;
                alternativeDtoList.Add(alternativeDto);
            }
            nextQuizQuestion.Alternatives = alternativeDtoList;
            nextQuizQuestion.QuizQuestionId = quizQuestion.Id;
            nextQuizQuestion.Question = quizQuestion.Question;
            nextQuizQuestion.CorrectAlternative = quizQuestion.CorrectAlternative;
            //var nextQuizQuestion = _mapper.Map<QuizQuestion, NextQuizQuestionDto>(quizQuestion);
            // Sjekker om det er siste quizquestion i quiz
            if ((request.quizQuestionIndex + 1) == Quiz.QuizQuestions.Count)
            {
                nextQuizQuestion.EndOfQuiz = true;
            }
            return nextQuizQuestion;
        }
    }

}
