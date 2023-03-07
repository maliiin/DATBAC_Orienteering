using MediatR;
using Microsoft.EntityFrameworkCore;
using Microsoft.Extensions.Hosting;
using orienteering_backend.Core.Domain.Checkpoint;
using orienteering_backend.Core.Domain.Checkpoint.Dto;
using orienteering_backend.Core.Domain.Checkpoint.Pipelines;
using orienteering_backend.Infrastructure.Data;
using orienteering_backend.Core.Domain.Quiz;
using orienteering_backend.Core.Domain.Quiz.Dto;
using System;
using orienteering_backend.Core.Domain.Quiz.Pipelines;
using AutoMapper;
using orienteering_backend.Infrastructure.Automapper;
using orienteering_backend;
using Newtonsoft.Json;
// Kilder: https://thecodeblogger.com/2021/07/07/in-memory-database-provider-for-testing-net-ef-core-app/ (17.02.2023)

namespace orienteering_backend.Tests.Helpers;
public class InMemoryTest
{
    public readonly DbContextOptions<OrienteeringContext> dbContextOptions;

    private static IMapper _mapper;
    public InMemoryTest()
    {
        // Build DbContextOptions
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

    //[Fact]

    //public async Task CreateCheckpointTest()
    //{
    //    //var inMemoryTest = new InMemoryTest();
    //    var _db = new OrienteeringContext(dbContextOptions, null);
    //    if (!_db.Database.IsInMemory())
    //    {
    //        _db.Database.Migrate();
    //    }


    //    var trackId = new Guid();
    //    Checkpoint checkpoint = new("test1", 0, trackId);
    //    CheckpointDto checkpointDto = new("test1", trackId, 0);

    //    var request = new CreateCheckpoint.Request(checkpointDto);
    //    var handler = new CreateCheckpoint.Handler(_db);
    //    var checkpointId = handler.Handle(request, CancellationToken.None).GetAwaiter().GetResult();
    //    var result = await _db.Checkpoints.FirstOrDefaultAsync(c => c.Id == checkpointId);
    //    Assert.Equal(checkpoint, result);
    //}

    [Fact]

    public async Task GetSingleCheckpointTest()
    {
        //var inMemoryTest = new InMemoryTest();
        var _db = new OrienteeringContext(dbContextOptions, null);
        if (!_db.Database.IsInMemory())
        {
            _db.Database.Migrate();
        }


        var trackId = Guid.NewGuid();
        var checkpoint = new Checkpoint("test1", 0, trackId);
        CheckpointDto checkpointDto = new("test1", trackId, 0);
        await _db.Checkpoints.AddAsync(checkpoint);
        await _db.SaveChangesAsync();
        var request = new GetSingleCheckpoint.Request(checkpoint.Id);
        var handler = new GetSingleCheckpoint.Handler(_db, _mapper);
        var returnedDto = handler.Handle(request, CancellationToken.None).GetAwaiter().GetResult();
        Assert.Equal(checkpointDto.Title, returnedDto.Title);
        Assert.Equal(checkpointDto.TrackId, returnedDto.TrackId);
        Assert.Equal(checkpointDto.GameId, returnedDto.GameId);

    }

    [Fact]

    public async Task GetNextQuizQuestionTest()
    {
        var _db = new OrienteeringContext(dbContextOptions, null);
        if (!_db.Database.IsInMemory())
        {
            _db.Database.Migrate();
        }

        var quizId = Guid.NewGuid();
        var quiz = new Quiz(quizId);
        var quizQuestion = new QuizQuestion();
        quizQuestion.Question = "sporsmol";
        quizQuestion.CorrectAlternative = 1;
        var alt1 = new Alternative(1, "grøn");
        var alt2 = new Alternative(2, "rød");
        var alt3 = new Alternative(3, "blå");
        quizQuestion.Alternatives.Add(alt1);
        quizQuestion.Alternatives.Add(alt2);
        quizQuestion.Alternatives.Add(alt3);
        quiz.AddQuizQuestion(quizQuestion);
        await _db.Quiz.AddAsync(quiz);
        await _db.SaveChangesAsync();
        var request = new GetNextQuizQuestion.Request(quiz.Id, 0);
        var handler = new GetNextQuizQuestion.Handler(_db, _mapper);
        var returnedDto = handler.Handle(request, CancellationToken.None).GetAwaiter().GetResult();
        var nextQuizQuestion = _mapper.Map<QuizQuestion, NextQuizQuestionDto>(quizQuestion);
        nextQuizQuestion.EndOfQuiz = true;
        Assert.Equal(JsonConvert.SerializeObject(returnedDto), JsonConvert.SerializeObject(nextQuizQuestion));



    }

    [Fact]

    public async Task test1()
    {
        var a = 1;
        var b = a; 
        Assert.Equal(a, b);
    }
}
