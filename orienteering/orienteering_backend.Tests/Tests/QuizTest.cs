using AutoMapper;
using Microsoft.EntityFrameworkCore;
using Moq;
using Newtonsoft.Json;
using orienteering_backend.Core.Domain.Track.Dto;
using orienteering_backend.Core.Domain.Track.Pipelines;
using orienteering_backend.Core.Domain.Track;
using orienteering_backend.Infrastructure.Automapper;
using orienteering_backend.Infrastructure.Data;
using orienteering_backend.Core.Domain.Quiz;
using orienteering_backend.Core.Domain.Quiz.Dto;
using MediatR;
using orienteering_backend.Core.Domain.Quiz.Pipelines;
using orienteering_backend.Core.Domain.Authentication.Services;
using orienteering_backend.Core.Domain.Checkpoint.Pipelines;
using orienteering_backend.Core.Domain.Checkpoint;
using Xunit;
// license Xunit (Apache 2.0): https://github.com/xunit/xunit/blob/main/LICENSE
// License MediatR (Apache 2.0): https://github.com/jbogard/MediatR/blob/master/LICENSE
// License Automapper (MIT): https://github.com/AutoMapper/AutoMapper/blob/master/LICENSE.txt


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

            // "Mocker" automapper
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
            //ARRANGE
            var _db = new OrienteeringContext(dbContextOptions);
            if (!_db.Database.IsInMemory()) { _db.Database.Migrate(); }

            var userId = Guid.NewGuid();
            var user = new TrackUserIdDto();
            user.UserId = userId;

            var _identityService = new Mock<IIdentityService>();
            _identityService.Setup(i => i.GetCurrentUserId()).Returns(userId);

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
            var handler = new AddQuizQuestion.Handler(_db, _identityService.Object, mediator.Object);

            //act
            var response = handler.Handle(request, CancellationToken.None).GetAwaiter().GetResult();

            //assert
            Assert.True(response);
            var quizDb = await _db.Quiz.Where(q => q.Id == quizId).FirstOrDefaultAsync();
            Assert.NotNull(quizDb);
        }

        [Fact]
        public async Task GivenSignedInUser_WhenDeleteQuestion_ThenDeleteQuestion()
        {
            //ARRANGE
            var _db = new OrienteeringContext(dbContextOptions);
            if (!_db.Database.IsInMemory()) { _db.Database.Migrate(); }

            var userId = Guid.NewGuid();
            var user = new TrackUserIdDto();
            user.UserId = userId;

            var _identityService = new Mock<IIdentityService>();
            _identityService.Setup(i => i.GetCurrentUserId()).Returns(userId);

            var mediator = new Mock<IMediator>();
            mediator.Setup(m => m.Send(It.IsAny<GetTrackUserByQuiz.Request>(), It.IsAny<CancellationToken>())).ReturnsAsync(user);

            //create quiz
            var quizId = Guid.NewGuid();
            var quiz = new Quiz(quizId);
            var quizQuestion = new QuizQuestion();
            quizQuestion.Question = "question?";
            quizQuestion.CorrectAlternative = 2;
            var alternatives = new List<Alternative>();
            alternatives.Add(new Alternative(1, "alternative1"));
            alternatives.Add(new Alternative(2, "alternative2"));
            alternatives.Add(new Alternative(3, "alternative3"));
            quizQuestion.Alternatives = alternatives;
            quiz.AddQuizQuestion(quizQuestion);
            //add quiz to db
            await _db.Quiz.AddAsync(quiz);
            await _db.SaveChangesAsync();

            var quizDb = await _db.Quiz
                .Where(q => q.Id == quizId)
                .Include(q => q.QuizQuestions)
                .FirstOrDefaultAsync();
            var quizQuestionId = quizDb.QuizQuestions[0].Id;

            var request = new DeleteQuizQuestion.Request(quizQuestionId);
            var handler = new DeleteQuizQuestion.Handler(_db, _identityService.Object, mediator.Object);

            //act
            var response = handler.Handle(request, CancellationToken.None).GetAwaiter().GetResult();

            //assert
            Assert.True(response);
            //var quizDb = await _db.Quiz.Where(q => q.Id == quizId).FirstOrDefaultAsync();
            Assert.NotNull(quizDb);
            Assert.Empty(quizDb.QuizQuestions);
        }

        [Fact]
        public async Task GivenCorrectUser_WhenAskForQuiz_ThenReturnQuiz()
        {
            //ARRANGE
            var _db = new OrienteeringContext(dbContextOptions);
            if (!_db.Database.IsInMemory()) { _db.Database.Migrate(); }

            var userId = Guid.NewGuid();
            TrackUserIdDto trackUser = new();
            trackUser.UserId = userId;
            
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

            //mock
            var _identityService = new Mock<IIdentityService>();
            _identityService.Setup(i => i.GetCurrentUserId()).Returns(userId);

            var _mediator = new Mock<IMediator>();
            _mediator.Setup(m => m.Send(It.IsAny<GetTrackUserByQuiz.Request>(), It.IsAny<CancellationToken>())).ReturnsAsync(trackUser);

            var request = new GetQuiz.Request(quizId);
            var handler = new GetQuiz.Handler(_db, _mapper, _identityService.Object, _mediator.Object);

            //ACT
            var returnedQuizDto = handler.Handle(request, CancellationToken.None).GetAwaiter().GetResult();

            //ASSERT
            Assert.Equal(JsonConvert.SerializeObject(quizDto), JsonConvert.SerializeObject(returnedQuizDto));
        }

        [Fact]
        public async Task GivenCheckpointId_WhenGetQuizByCheckpointId()
        {
            //ARRANGE
            var _db = new OrienteeringContext(dbContextOptions);
            if (!_db.Database.IsInMemory()) { _db.Database.Migrate(); }

            //create track
            var track = new Track();
            track.UserId = Guid.NewGuid();
            track.Name = "trackname";
            await _db.Tracks.AddAsync(track);
            await _db.SaveChangesAsync();

            //create Quiz
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

            //quizDto
            var dtoList = new List<QuizQuestionDto>();
            for (var i = 0; i < quiz.QuizQuestions.Count; i++)
            {
                var quizQuestion1 = quiz.QuizQuestions[i];
                var quizQuestionDto = _mapper.Map<QuizQuestion, QuizQuestionDto>(quizQuestion1);
                dtoList.Add(quizQuestionDto);
            }
            var quizDto = new QuizDto(quiz.Id, dtoList);

            //create checkpoint
            var checkpoint = new Checkpoint("test10", 0, track.Id);
            checkpoint.QuizId = quizId;
            checkpoint.CheckpointDescription = "Test";
            await _db.Checkpoints.AddAsync(checkpoint);
            track.AddedCheckpoint();
            await _db.SaveChangesAsync();

            var _mediator = new Mock<IMediator>();
            _mediator.Setup(m => m.Send(It.IsAny<GetQuizIdOfCheckpoint.Request>(), It.IsAny<CancellationToken>())).ReturnsAsync(quizId);

            var request = new GetQuizByCheckpointId.Request(quizId);
            var handler = new GetQuizByCheckpointId.Handler(_db, _mapper, _mediator.Object);

            //act
            var response = handler.Handle(request, CancellationToken.None).GetAwaiter().GetResult();

            //ASSERT
            Assert.Equal(JsonConvert.SerializeObject(quizDto), JsonConvert.SerializeObject(response));
        }

        [Fact]
        public void GivenQuiz_WhenAddQuizQuestion_ThenIncreaceQuestions()
        {
            //arrange
            var alternative1 = new Alternative(1, "alternative1");
            var alternative2 = new Alternative(2, "alternative2");
            var quizQuestion = new QuizQuestion();
            quizQuestion.Alternatives.Add(alternative1);
            quizQuestion.Alternatives.Add(alternative2);
            var quiz = new Quiz(Guid.NewGuid());

            //act
            quiz.AddQuizQuestion(quizQuestion);

            //assert
            Assert.Single(quiz.QuizQuestions);
        }

        [Fact]
        public async Task GivenQuiz_WhenRemoveQuizQuestion_ThenRemoveQuestions()
        {
            //arrange
            var _db = new OrienteeringContext(dbContextOptions);
            if (!_db.Database.IsInMemory()) { _db.Database.Migrate(); }

            var alternative1 = new Alternative(1, "alternative1");
            var alternative2 = new Alternative(2, "alternative2");
            var quizQuestion1 = new QuizQuestion();
            quizQuestion1.Alternatives.Add(alternative1);
            quizQuestion1.Alternatives.Add(alternative2);

            var alternative3 = new Alternative(1, "alternative1");
            var alternative4 = new Alternative(2, "alternative2");
            var quizQuestion2 = new QuizQuestion();
            quizQuestion2.Alternatives.Add(alternative3);
            quizQuestion2.Alternatives.Add(alternative4);

            var quiz = new Quiz(Guid.NewGuid());
            quiz.AddQuizQuestion(quizQuestion1);
            quiz.AddQuizQuestion(quizQuestion2);

            await _db.Quiz.AddAsync(quiz);
            await _db.SaveChangesAsync();

            //act
            var result = quiz.RemoveQuizQuestion(quizQuestion1.Id);

            //assert
            Assert.Single(quiz.QuizQuestions);
            Assert.True(result);
        }

        [Fact]
        public void GivenEmptyQuiz_WhenRemoveQuizQuestion_ThenReturnFalse()
        {
            //arrange
            var quiz = new Quiz(Guid.NewGuid());

            //act
            var result = quiz.RemoveQuizQuestion(Guid.NewGuid());

            //assert
            Assert.Empty(quiz.QuizQuestions);
            Assert.False(result);
        }
    }
}
