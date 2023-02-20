using MediatR;
using Microsoft.EntityFrameworkCore;
using orienteering_backend.Infrastructure.Data;
using orienteering_backend.Core.Domain.Quiz.Dto;
//Kilder: CampusEats lab fra dat240
// Kilder: https://github.com/dat240-2022/assignments/blob/main/Lab3/UiS.Dat240.Lab3/Core/Domain/Cart/Pipelines/AddItem.cs (07.02.2023)
// Brukte samme struktur på pipelinen som i kilden

namespace orienteering_backend.Core.Domain.Quiz.Pipelines;

public static class AddQuizQuestion
{
    public record Request(
        InputCreateQuestionDto inputCreateQuestionDto
        ) : IRequest<bool>;


    public class Handler : IRequestHandler<Request, bool>
    {
        private readonly OrienteeringContext _db;
        private readonly IMediator _mediator;

        public Handler(OrienteeringContext db, IMediator mediator)
        {
            _db = db ?? throw new ArgumentNullException(nameof(db));
            _mediator = mediator;
        }
        public async Task<bool> Handle(Request request, CancellationToken cancellationToken)
        {
            Guid QuizGuid = new Guid(request.inputCreateQuestionDto.QuizId);
            Console.WriteLine($"guid of quiz is {QuizGuid}\n\n");
            var Quiz = await _db.Quiz
                .FirstOrDefaultAsync(q => q.Id == QuizGuid, cancellationToken);


            if (Quiz == null)
            {
                Console.WriteLine("quiz er null");
                return false;
            }

            //dette er testing som virker
            //var test = new QuizQuestion("dette er spm111112", 2);
            //test.Alternatives.Add(new Alternative(1, "dette er alternativ1"));
            //test.Alternatives.Add(new Alternative(2, "dette er alternativ2122"));

            //test uten å sette id
            //test.Alternatives.Add(new Alternative(1, "dette er alternativ1"));
            //test.Alternatives.Add(new Alternative(2, "dette er alternativ2122"));

            //Quiz.AddQuizQuestion(test);




            var quizQuestion = new QuizQuestion(request.inputCreateQuestionDto.Question, request.inputCreateQuestionDto.CorrectAlternative);

            List<Alternative> alternatives = new List<Alternative>();
            foreach (var dto in request.inputCreateQuestionDto.Alternatives)
            {
                alternatives.Add(new Alternative(dto.Id, dto.Text));
                //alternatives.Append(new Alternative(dto.Text));
            }
            quizQuestion.Alternatives = alternatives;
            Console.WriteLine($"the whole quiz {Quiz}\n\n");

            Quiz.AddQuizQuestion(quizQuestion);

            //try
            //{
            await _db.SaveChangesAsync();
            //    // move on
            //}
            //catch (DbUpdateException)
            //{
            //    Console.WriteLine("\n\n\n\n\n\n\nfungerer ikke"); // get latest version of record for display
            //    return false;
            //}
            return true;
            //await _db.SaveChangesAsync();
            //return true;



            //gammel- fungerer ikke!!! error

            //Guid QuizGuid = new Guid(request.inputCreateQuestionDto.QuizId);
            //Console.WriteLine($"guid of quiz is {QuizGuid}\n\n");
            //var Quiz = await _db.Quiz
            //    .FirstOrDefaultAsync(q => q.Id == QuizGuid, cancellationToken);


            //if (Quiz == null)
            //{
            //    Console.WriteLine("quiz er null");
            //    return false;
            //}
            //var quizQuestion = new QuizQuestion(request.inputCreateQuestionDto.Question, request.inputCreateQuestionDto.CorrectAlternative);

            //List<Alternative> alternatives = new List<Alternative>();
            //foreach (var dto in request.inputCreateQuestionDto.Alternatives)
            //{
            //    alternatives.Add(new Alternative(dto.Id, dto.Text));
            //    //alternatives.Append(new Alternative(dto.Text));
            //}
            //quizQuestion.Alternatives = alternatives;
            //Console.WriteLine($"the whole quiz {Quiz}\n\n");

            //Quiz.AddQuizQuestion(quizQuestion);

            //try
            //{
            //    await _db.SaveChangesAsync();
            //    // move on
            //}
            //catch (DbUpdateException)
            //{
            //    Console.WriteLine("\n\n\n\n\n\n\n fungerer ikke"); // get latest version of record for display
            //    return false;
            //}
            //return true;
            ////await _db.SaveChangesAsync();
            ////return true;
        }
    }
}
