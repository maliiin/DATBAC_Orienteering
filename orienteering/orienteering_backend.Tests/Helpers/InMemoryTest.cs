using MediatR;
using System;
using System.Text;
using Microsoft.EntityFrameworkCore;
using Microsoft.Extensions.Hosting;
using orienteering_backend;
using orienteering_backend.Core.Domain.Checkpoint;
using orienteering_backend.Core.Domain.Checkpoint.Dto;
using orienteering_backend.Core.Domain.Checkpoint.Pipelines;
using orienteering_backend.Infrastructure.Data;
using orienteering_backend.Core.Domain.Quiz;
using orienteering_backend.Core.Domain.Quiz.Dto;
using orienteering_backend.Core.Domain.Quiz.Pipelines;
using AutoMapper;
using orienteering_backend.Infrastructure.Automapper;
using Newtonsoft.Json;
using orienteering_backend.Core.Domain.Track;
using orienteering_backend.Core.Domain.Track.Dto;
using orienteering_backend.Core.Domain.Track.Pipelines;

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

    //[Fact]

    //public async Task GenerateQRTest()
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
    public async Task GetCheckpointForTracksTest()
    {
        var _db = new OrienteeringContext(dbContextOptions, null);
        if (!_db.Database.IsInMemory())
        {
            _db.Database.Migrate();
        }

        var trackId = Guid.NewGuid();

        var checkpoint1 = new Checkpoint("test1", 0, trackId);
        var checkpoint2 = new Checkpoint("test2", 0, trackId);

        await _db.Checkpoints.AddAsync(checkpoint1);
        await _db.Checkpoints.AddAsync(checkpoint2);
        await _db.SaveChangesAsync();
        var request = new GetCheckpointsForTrack.Request(trackId);
        var handler = new GetCheckpointsForTrack.Handler(_db, _mapper);
        var returnedDtoList = handler.Handle(request, CancellationToken.None).GetAwaiter().GetResult();
        var checkpoint1Dto = _mapper.Map<Checkpoint, CheckpointDto>(checkpoint1);
        var checkpoint2Dto = _mapper.Map<Checkpoint, CheckpointDto>(checkpoint2);
        Assert.Equal(JsonConvert.SerializeObject(checkpoint1Dto), JsonConvert.SerializeObject(returnedDtoList[0]));
        Assert.Equal(JsonConvert.SerializeObject(checkpoint2Dto), JsonConvert.SerializeObject(returnedDtoList[1]));
    }

    //[Fact]
    //public async Task GetQRCodesTest()
    //{
    //    var _db = new OrienteeringContext(dbContextOptions, null);
    //    if (!_db.Database.IsInMemory())
    //    {
    //        _db.Database.Migrate();
    //    }

    //    var trackId = Guid.NewGuid();

    //    var checkpoint1 = new Checkpoint("test1", 0, trackId);
    //    checkpoint1.QRCode = "dGVzdD" 
    //    var checkpoint2 = new Checkpoint("test2", 0, trackId);

    //    await _db.Checkpoints.AddAsync(checkpoint1);
    //    await _db.Checkpoints.AddAsync(checkpoint2);
    //    await _db.SaveChangesAsync();
    //    var request = new GetCheckpointsForTrack.Request(trackId);
    //    var handler = new GetCheckpointsForTrack.Handler(_db, _mapper);
    //    var returnedDtoList = handler.Handle(request, CancellationToken.None).GetAwaiter().GetResult();
    //    var checkpoint1Dto = _mapper.Map<Checkpoint, CheckpointDto>(checkpoint1);
    //    var checkpoint2Dto = _mapper.Map<Checkpoint, CheckpointDto>(checkpoint2);
    //    Assert.Equal(JsonConvert.SerializeObject(checkpoint1Dto), JsonConvert.SerializeObject(returnedDtoList[0]));
    //    Assert.Equal(JsonConvert.SerializeObject(checkpoint2Dto), JsonConvert.SerializeObject(returnedDtoList[1]));
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
    public async Task AddQuizQuestionTest()
    {
        var _db = new OrienteeringContext(dbContextOptions, null);
        if (!_db.Database.IsInMemory())
        {
            _db.Database.Migrate();
        }

        var quizIdString = System.Guid.NewGuid().ToString();
        var quizIdGuid = new Guid(quizIdString);
        var quiz = new Quiz(quizIdGuid);
        var quizQuestion = new QuizQuestion();
        quizQuestion.Question = "question";
        quizQuestion.CorrectAlternative = 1;
        var alt1 = new Alternative(1, "green");
        var alt2 = new Alternative(2, "red");
        quizQuestion.Alternatives.Add(alt1);
        quizQuestion.Alternatives.Add(alt2);
        var alternativeDtoList = new List<AlternativeDto>();
        alternativeDtoList.Add(_mapper.Map<Alternative, AlternativeDto>(alt1));
        alternativeDtoList.Add(_mapper.Map<Alternative, AlternativeDto>(alt2));
        await _db.Quiz.AddAsync(quiz);
        await _db.SaveChangesAsync();
        var inputCreateQuestionDto = new InputCreateQuestionDto(quizQuestion.Question, alternativeDtoList, quizQuestion.CorrectAlternative, quizIdString);
        quiz.QuizQuestions.Add(quizQuestion);
        var request = new AddQuizQuestion.Request(inputCreateQuestionDto);
        var handler = new AddQuizQuestion.Handler(_db);
        var result = handler.Handle(request, CancellationToken.None).GetAwaiter().GetResult();
        Assert.True(result);
        var addedQuiz = await _db.Quiz.Include(a => a.QuizQuestions).ThenInclude(b => b.Alternatives).FirstOrDefaultAsync(q => q.Id == quizIdGuid);
        Assert.Equal(JsonConvert.SerializeObject(quiz), JsonConvert.SerializeObject(addedQuiz));
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
        quizQuestion.Question = "question";
        quizQuestion.CorrectAlternative = 1;
        var alt1 = new Alternative(1, "green");
        var alt2 = new Alternative(1, "red");
        quizQuestion.Alternatives.Add(alt1);
        quizQuestion.Alternatives.Add(alt2);
        quiz.QuizQuestions.Add(quizQuestion);

        await _db.Quiz.AddAsync(quiz);
        await _db.SaveChangesAsync();
        var request = new GetNextQuizQuestion.Request(quiz.Id, 0);
        var handler = new GetNextQuizQuestion.Handler(_db, _mapper);
        var returnedDto = handler.Handle(request, CancellationToken.None).GetAwaiter().GetResult();
        //var nextQuizQuestion = _mapper.Map<QuizQuestion, NextQuizQuestionDto>(quizQuestion);

        var nextQuizQuestion = new NextQuizQuestionDto();
        var alternativeDtoList = new List<AlternativeDto>();
        var alternative1Dto = _mapper.Map<Alternative, AlternativeDto>(alt1);
        var alternative2Dto = _mapper.Map<Alternative, AlternativeDto>(alt2);
        alternativeDtoList.Add(alternative1Dto);
        alternativeDtoList.Add(alternative2Dto);
        nextQuizQuestion.Alternatives = alternativeDtoList;
        nextQuizQuestion.QuizQuestionId = quizQuestion.Id;
        nextQuizQuestion.Question = quizQuestion.Question;

        nextQuizQuestion.EndOfQuiz = true;
        Assert.Equal(JsonConvert.SerializeObject(returnedDto), JsonConvert.SerializeObject(nextQuizQuestion));
    }

    [Fact]
    public async Task GetQuizTest()
    {
        var _db = new OrienteeringContext(dbContextOptions, null);
        if (!_db.Database.IsInMemory())
        {
            _db.Database.Migrate();
        }
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
        var request = new GetQuiz.Request(quizId);
        var handler = new GetQuiz.Handler(_db, _mapper);
        var returnedQuizDto = handler.Handle(request, CancellationToken.None).GetAwaiter().GetResult();

        var quizQuestionDto = new QuizQuestionDto();
        var alternativeDtoList = new List<AlternativeDto>();
        alternativeDtoList.Add(_mapper.Map<Alternative, AlternativeDto>(alt1));
        alternativeDtoList.Add(_mapper.Map<Alternative, AlternativeDto>(alt2));
        quizQuestionDto.Alternatives = alternativeDtoList;
        quizQuestionDto.QuizQuestionId = quizQuestion.Id;
        quizQuestionDto.Question = quizQuestion.Question;
        quizQuestionDto.CorrectAlternative = quizQuestion.CorrectAlternative;

        var quizDto = new QuizDto(quizId, new List<QuizQuestionDto> { quizQuestionDto });

        Assert.Equal(JsonConvert.SerializeObject(returnedQuizDto), JsonConvert.SerializeObject(quizDto));
    }

    [Fact]
    public async Task GetSolutionTest()
    {
        var _db = new OrienteeringContext(dbContextOptions, null);
        if (!_db.Database.IsInMemory())
        {
            _db.Database.Migrate();
        }
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
        var request = new GetSolution.Request(quizId, quizQuestion.Id);
        var handler = new GetSolution.Handler(_db);
        var result = handler.Handle(request, CancellationToken.None).GetAwaiter().GetResult();

        Assert.Equal("green", result);
    }

    [Fact]
    public async Task CreateTrackTest()
    {
        var _db = new OrienteeringContext(dbContextOptions, null);
        if (!_db.Database.IsInMemory())
        {
            _db.Database.Migrate();
        }
        var trackDto = new TrackDto();
        trackDto.UserId = Guid.NewGuid();
        trackDto.TrackName = "Test";
        trackDto.TrackId = Guid.NewGuid();
        var request = new CreateTrack.Request(trackDto);
        var handler = new CreateTrack.Handler(_db, _mapper);
        var result = handler.Handle(request, CancellationToken.None).GetAwaiter().GetResult();
        Assert.Equal(result, trackDto.TrackId);

        var addedTrack = await _db.Tracks.FirstOrDefaultAsync(t => t.Id == trackDto.TrackId);
        Assert.NotNull(addedTrack);
        var addedTrackDto = _mapper.Map<Track, TrackDto>(addedTrack);
        Assert.Equal(JsonConvert.SerializeObject(trackDto), JsonConvert.SerializeObject(addedTrackDto));
    }

    [Fact]
    public async Task GetSingleTrackTest()
    {
        var _db = new OrienteeringContext(dbContextOptions, null);
        if (!_db.Database.IsInMemory())
        {
            _db.Database.Migrate();
        }
        var track = new Track();
        track.UserId = Guid.NewGuid();
        track.Name = "Test";
        await _db.Tracks.AddAsync(track);
        await _db.SaveChangesAsync();
        var request = new GetSingleTrack.Request(track.Id);
        var handler = new GetSingleTrack.Handler(_db, _mapper);
        var returnedTrackDto = handler.Handle(request, CancellationToken.None).GetAwaiter().GetResult();
        var trackDto = _mapper.Map<Track, TrackDto>(track);
        Assert.Equal(JsonConvert.SerializeObject(trackDto), JsonConvert.SerializeObject(returnedTrackDto));
    }

    [Fact]
    public async Task GetTracksTest()
    {
        var _db = new OrienteeringContext(dbContextOptions, null);
        if (!_db.Database.IsInMemory())
        {
            _db.Database.Migrate();
        }
        var userId = Guid.NewGuid();
        var track1 = new Track();
        track1.UserId = userId;
        track1.Name = "Test1";
        var track2 = new Track();
        track2.UserId = userId;
        track2.Name = "Test2";
        await _db.Tracks.AddAsync(track1);
        await _db.Tracks.AddAsync(track2);
        await _db.SaveChangesAsync();
        var request = new GetTracks.Request(userId);
        var handler = new GetTracks.Handler(_db, _mapper);
        var returnedTrackDtoList = handler.Handle(request, CancellationToken.None).GetAwaiter().GetResult();
        var track1Dto = _mapper.Map<Track, TrackDto>(track1);
        var track2Dto = _mapper.Map<Track, TrackDto>(track2);
        var tracDtoList = new List<TrackDto> {track1Dto, track2Dto};
        Assert.Equal(JsonConvert.SerializeObject(tracDtoList), JsonConvert.SerializeObject(returnedTrackDtoList));
    }


}
