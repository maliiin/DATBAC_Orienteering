using AutoMapper;
using Microsoft.EntityFrameworkCore;
using Moq;
using Newtonsoft.Json;
using orienteering_backend.Core.Domain.Track.Dto;
using orienteering_backend.Core.Domain.Track.Pipelines;
using orienteering_backend.Core.Domain.Track;
using orienteering_backend.Infrastructure.Automapper;
using orienteering_backend.Infrastructure.Data;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using orienteering_backend.Core.Domain.Quiz;
using orienteering_backend.Core.Domain.Quiz.Dto;
using MediatR;
using orienteering_backend.Core.Domain.Quiz.Pipelines;
using orienteering_backend.Core.Domain.Authentication.Services;

namespace orienteering_backend.Tests.Helpers
{
    public class QuizTest
    {
        public readonly DbContextOptions<OrienteeringContext> dbContextOptions;

        private static IMapper _mapper;
        public QuizTest()
        {
            dbContextOptions = new DbContextOptionsBuilder<OrienteeringContext>()
               .UseInMemoryDatabase(databaseName: "orienteeringTest")
               .Options;

            // "Mocker" automapper Fix bruker mock nå heller eller ikke?
            //Kilder: https://www.thecodebuzz.com/unit-test-mock-automapper-asp-net-core-imapper/ (06.03.2023)
            if (_mapper == null)
            {
                var mappingConfig = new MapperConfiguration(mc =>
                {
                    mc.AddProfile(new MapperProfile());
                });
                IMapper mapper = mappingConfig.CreateMapper();
                _mapper = mapper;
            }
        }

        [Fact]
        public async Task GivenSignedInUser_WhenAddQuestion_ThenAddQuestion()
        {
            //arrange
            var _db = new OrienteeringContext(dbContextOptions, null);
            if (!_db.Database.IsInMemory()) { _db.Database.Migrate(); }

            var userId = Guid.NewGuid();
            var user = new TrackUserIdDto();
            user.UserId=userId;

            var identityService = new Mock<IIdentityService>();
            identityService.Setup(i => i.GetCurrentUserId()).Returns(userId);

            var mediator = new Mock<IMediator>();
            mediator.Setup(m => m.Send(It.IsAny<GetTrackUserByQuiz.Request>(), It.IsAny<CancellationToken>())).ReturnsAsync(user);

            var quizId = Guid.NewGuid();
            var quiz = new Quiz(quizId);
            await _db.Quiz.AddAsync(quiz);
            await _db.SaveChangesAsync();

            //input
            var alternativesDto = new List<AlternativeDto>();
            alternativesDto.Add(new AlternativeDto("alternative1", 1));
            alternativesDto.Add(new AlternativeDto("alternative2", 2));
            alternativesDto.Add(new AlternativeDto("alternative3", 3));
            var questionDto = new InputCreateQuestionDto("question string?", alternativesDto, 2, quizId.ToString());


            var request = new AddQuizQuestion.Request(questionDto);
            var handler = new AddQuizQuestion.Handler(_db, identityService.Object, mediator.Object);

            //act
            var response = handler.Handle(request, CancellationToken.None).GetAwaiter().GetResult();

            //assert
            Assert.True(response);
            var quizDb = await _db.Quiz.Where(q => q.Id == quizId).FirstOrDefaultAsync();
            Assert.NotNull(quizDb);

            //fix-hvor mye skal sjekkes? skal vi sjekke at det som kommer ut av db stemmer med det som ble puttet inn?
            var quizQuestion = quizDb.QuizQuestions[0];



            ////add track to db
            //var track = new Track();
            //track.Name = "name";
            //track.UserId = Guid.NewGuid();
            ////track.UserId = Guid.NewGuid();
            //await _db.Tracks.AddAsync(track);
            //await _db.SaveChangesAsync();

            //var trackDb = await _db.Tracks.Where(t => t.Name == "name").FirstOrDefaultAsync();
            //var expected = new TrackUserIdDto();
            //expected.UserId = track.UserId;
            //expected.TrackId = trackDb.Id;

            ////var mapper = _mapper;
            //var mapper = new Mock<IMapper>();
            //mapper.Setup(x => x.Map<Track, TrackUserIdDto>(track)).Returns(expected);

            //var request = new GetTrackUser.Request(trackDb.Id);
            //var handler = new GetTrackUser.Handler(_db, mapper.Object);

            ////act
            //var response = handler.Handle(request, CancellationToken.None).GetAwaiter().GetResult();
            //Assert.Equal(JsonConvert.SerializeObject(expected), JsonConvert.SerializeObject(response));

        }

        [Fact]
        public async Task GivenSignedInUser_WhenDeleteQuestion_ThenDeleteQuestion()
        {
            //arrange
            var _db = new OrienteeringContext(dbContextOptions, null);
            if (!_db.Database.IsInMemory()) { _db.Database.Migrate(); }

            var userId = Guid.NewGuid();
            var user = new TrackUserIdDto();
            user.UserId = userId;

            var identityService = new Mock<IIdentityService>();
            identityService.Setup(i => i.GetCurrentUserId()).Returns(userId);

            var mediator = new Mock<IMediator>();
            mediator.Setup(m => m.Send(It.IsAny<GetTrackUserByQuiz.Request>(), It.IsAny<CancellationToken>())).ReturnsAsync(user);

            //create quiz
            var quizId = Guid.NewGuid();
            var quiz = new Quiz(quizId);
            var quizQuestion = new QuizQuestion();
            quizQuestion.Question = "question?";
            quizQuestion.CorrectAlternative = 2;
            var alternatives = new List<Alternative>();
            alternatives.Add(new Alternative(1,"alternative1"));
            alternatives.Add(new Alternative(2,"alternative2"));
            alternatives.Add(new Alternative(3,"alternative3"));
            quizQuestion.Alternatives = alternatives;
            quiz.AddQuizQuestion(quizQuestion);
            //add quiz to db
            await _db.Quiz.AddAsync(quiz);
            await _db.SaveChangesAsync();

            var quizDb = await _db.Quiz
                .Where(q => q.Id == quizId)
                .Include(q=>q.QuizQuestions)
                .FirstOrDefaultAsync();
            var quizQuestionId = quizDb.QuizQuestions[0].Id;
          
            var request = new DeleteQuizQuestion.Request(quizQuestionId,quizId);
            var handler = new DeleteQuizQuestion.Handler(_db, identityService.Object, mediator.Object);

            //act
            var response = handler.Handle(request, CancellationToken.None).GetAwaiter().GetResult();

            //assert
            Assert.True(response);
            //var quizDb = await _db.Quiz.Where(q => q.Id == quizId).FirstOrDefaultAsync();
            Assert.NotNull(quizDb);
            Assert.Empty( quizDb.QuizQuestions);
           
        }


        [Fact]
        public async Task Given_WhenAskForQuiz_ThenReturnQuiz()
        {
            //ARRANGE
            var _db = new OrienteeringContext(dbContextOptions, null);
            if (!_db.Database.IsInMemory()) { _db.Database.Migrate(); }
            
            //create quiz and add to db
            var quizId = Guid.NewGuid();
            var quiz = new Quiz(quizId);
            var quizQuestion = new QuizQuestion();
            quizQuestion.Question = "question";
            quizQuestion.CorrectAlternative = 1;
            var alt1 = new Alternative(1, "green");
            var alt2 = new Alternative(2, "red");
            quizQuestion.Alternatives.Add(alt1);
            quizQuestion.Alternatives.Add(alt2);
            quiz.QuizQuestions.Add(quizQuestion);
            await _db.Quiz.AddAsync(quiz);
            await _db.SaveChangesAsync();

            //excpected values
            var quizQuestionDto = new QuizQuestionDto();
            var alternativeDtoList = new List<AlternativeDto>();
            alternativeDtoList.Add(_mapper.Map<Alternative, AlternativeDto>(alt1));
            alternativeDtoList.Add(_mapper.Map<Alternative, AlternativeDto>(alt2));
            quizQuestionDto.Alternatives = alternativeDtoList;
            quizQuestionDto.QuizQuestionId = quizQuestion.Id;
            quizQuestionDto.Question = quizQuestion.Question;
            quizQuestionDto.CorrectAlternative = quizQuestion.CorrectAlternative;
            var quizDto = new QuizDto(quizId, new List<QuizQuestionDto> { quizQuestionDto });

            var request = new GetQuiz.Request(quizId);
            var handler = new GetQuiz.Handler(_db, _mapper);

            //act
            var returnedQuizDto = handler.Handle(request, CancellationToken.None).GetAwaiter().GetResult();



            //ASSERT
            Assert.Equal(JsonConvert.SerializeObject(quizDto), JsonConvert.SerializeObject(returnedQuizDto));
        }


        //fix-mangler å teste getquizbycheckpointId pipeline
        //ellers ferdig i quiz domain
    }
}
