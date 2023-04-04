using MediatR;
using System;
using System.Text;
using Microsoft.EntityFrameworkCore;

using orienteering_backend.Infrastructure.Data;

using AutoMapper;
using orienteering_backend.Infrastructure.Automapper;
using Newtonsoft.Json;
using orienteering_backend.Core.Domain.Track;
using orienteering_backend.Core.Domain.Track.Dto;
using orienteering_backend.Core.Domain.Track.Pipelines;
using orienteering_backend.Core.Domain.Authentication.Services;
using Moq;
using Microsoft.AspNetCore.Http;
using System.Security.Claims;
using System.Security.Principal;
using orienteering_backend.Core.Domain.Authentication;
using Microsoft.AspNetCore.Identity;
using Microsoft.Extensions.Logging;
using Microsoft.Extensions.Options;
using Microsoft.AspNetCore.Authentication;
using System.Net.NetworkInformation;
using System.Security.Authentication;

// Kilder: https://thecodeblogger.com/2021/07/07/in-memory-database-provider-for-testing-net-ef-core-app/ (17.02.2023)

namespace orienteering_backend.Tests.Helpers;
//fra kilde https://stackoverflow.com/questions/49165810/how-to-mock-usermanager-in-net-core-testing 31.03


public class InMemoryTest
{
    public readonly DbContextOptions<OrienteeringContext> dbContextOptions;

    private static IMapper _mapper;
    private readonly Mock<IMediator> _mediator;
    private readonly Mock<IIdentityService> _identityService;
    //private readonly Mock<IMapper> _mapper;



    //private readonly UserManager<IdentityUser> _userManager;
    private readonly Mock<UserManager<IdentityUser>> _userManager;
    private readonly Mock<SignInManager<IdentityUser>> _signInManager;
    private readonly Mock<IHttpContextAccessor> _httpContextAccessor;
    private readonly Mock<FakeSignInManager> _testSignInManager;
    public static UserManager<TUser> TestUserManager<TUser>(IUserStore<TUser> store = null) where TUser : class
    {
        store = store ?? new Mock<IUserStore<TUser>>().Object;
        var options = new Mock<IOptions<IdentityOptions>>();
        var idOptions = new IdentityOptions();
        idOptions.Lockout.AllowedForNewUsers = false;
        options.Setup(o => o.Value).Returns(idOptions);
        var userValidators = new List<IUserValidator<TUser>>();
        var validator = new Mock<IUserValidator<TUser>>();
        userValidators.Add(validator.Object);
        var pwdValidators = new List<PasswordValidator<TUser>>();
        pwdValidators.Add(new PasswordValidator<TUser>());
        var userManager = new UserManager<TUser>(store, options.Object, new PasswordHasher<TUser>(),
            userValidators, pwdValidators, new UpperInvariantLookupNormalizer(),
            new IdentityErrorDescriber(), null,
            new Mock<ILogger<UserManager<TUser>>>().Object);
        validator.Setup(v => v.ValidateAsync(userManager, It.IsAny<TUser>()))
            .Returns(Task.FromResult(IdentityResult.Success)).Verifiable();
        return userManager;
    }


    public static Mock<UserManager<TUser>> MockUserManager<TUser>() where TUser : class
    {
        var store = new Mock<IUserStore<TUser>>();
        var mgr = new Mock<UserManager<TUser>>(store.Object, null, null, null, null, null, null, null, null);
        mgr.Object.UserValidators.Add(new UserValidator<TUser>());
        mgr.Object.PasswordValidators.Add(new PasswordValidator<TUser>());

        mgr.Setup(x => x.DeleteAsync(It.IsAny<TUser>())).ReturnsAsync(IdentityResult.Success);
        mgr.Setup(x => x.CreateAsync(It.IsAny<TUser>(), It.IsAny<string>())).ReturnsAsync(IdentityResult.Success);//.Callback<TUser, string>((x, y) => ls.Add(x));
        mgr.Setup(x => x.UpdateAsync(It.IsAny<TUser>())).ReturnsAsync(IdentityResult.Success);

        return mgr;
    }

    public static Mock<IIdentityService> mockService<TUser>() where TUser : class
    {
        var serv = new Mock<IIdentityService>();
        //serv.Setup(x => x.CreateUser(It.IsAny<UserRegistration>())).Returns(Task<IdentityResult.Success>);
        serv.Setup(x => x.GetCurrentUserId());


        return serv;
    }

    //public class FakeSignInManager : SignInManager<IdentityUser>
    //{
    //    public FakeSignInManager()
    //            : base( MockUserManager<IdentityUser>().Object,
    //                 new Mock<IHttpContextAccessor>().Object,
    //                 new Mock<IUserClaimsPrincipalFactory<IdentityUser>>().Object,
    //                 new Mock<IOptions<IdentityOptions>>().Object,
    //                 new Mock<ILogger<SignInManager<IdentityUser>>>().Object,
    //                 new Mock<IAuthenticationSchemeProvider>().Object)
    //    { }
    //}

    public static Mock<SignInManager<IdentityUser>> MockSignInManager<TUser>() where TUser : class
    {
        var o = new Mock<SignInManager<IdentityUser>>();
        //o.Setup(x=>x.)
        return o;

    }
    public class FakeUserManager : UserManager<IdentityUser>
    {
        public FakeUserManager()
            : base(
                  new Mock<IUserStore<IdentityUser>>().Object,
                  new Mock<Microsoft.Extensions.Options.IOptions<IdentityOptions>>().Object,
                  new Mock<IPasswordHasher<IdentityUser>>().Object,
                  new IUserValidator<IdentityUser>[0],
                  new IPasswordValidator<IdentityUser>[0],
                  new Mock<ILookupNormalizer>().Object,
                  new Mock<IdentityErrorDescriber>().Object,
                  new Mock<IServiceProvider>().Object,
                  new Mock<ILogger<UserManager<IdentityUser>>>().Object)
        { }
    }

    public class FakeSignInManager : SignInManager<IdentityUser>
    {
        public FakeSignInManager()
            : base(
                  new Mock<FakeUserManager>().Object,
                  new HttpContextAccessor(),
                  new Mock<IUserClaimsPrincipalFactory<IdentityUser>>().Object,
                  new Mock<Microsoft.Extensions.Options.IOptions<IdentityOptions>>().Object,
                  new Mock<ILogger<SignInManager<IdentityUser>>>().Object,
                  new Mock<Microsoft.AspNetCore.Authentication.IAuthenticationSchemeProvider>().Object,
                  new Mock<Microsoft.AspNetCore.Identity.IUserConfirmation<IdentityUser>>().Object
                  )
        { }
    }

    public InMemoryTest()
    {

        var contextAccessor = new Mock<IHttpContextAccessor>();

        var userPrincipalFactory = new Mock<IUserClaimsPrincipalFactory<IdentityUser>>();

        _userManager = MockUserManager<IdentityUser>();

        //_userManager= new Mock<UserManager<IdentityUser>>(); //_userManager.Setup(x => x.)
        //_userManager = TestUserManager<IdentityUser>();
        //_signInManager = MockSignInManager<IdentityUser>();
        _httpContextAccessor = new Mock<IHttpContextAccessor>();

        //_signInManager = new Mock<SignInManager<IdentityUser>>(_userManager.Object, contextAccessor.Object, userPrincipalFactory.Object, null, null, null, null);
        //_signInManager= new Mock<SignInManager<IdentityUser>>();
        //_signInManager.Setup(x => x.UserManager).Returns(_userManager.Object);
        //_signInManager.Setup(x => x.Context).Returns(contextAccessor.Object);
        //_signInManager.Setup(x => x.ClaimsFactory).Returns(userPrincipalFactory.Object);
        //_signInManager.

        //_signInManager = new Mock<SignInManager<IdentityUser>>(MockBehavior.Default, _userManager.Object, contextAccessor.Object, userPrincipalFactory.Object, null, null, null, null);
        //_signInManager = new Mock<SignInManager<IdentityUser>>(
        //    _userManager.Object,
        //    /* IHttpContextAccessor contextAccessor */Mock.Of<IHttpContextAccessor>(),
        //    /* IUserClaimsPrincipalFactory<TUser> claimsFactory */Mock.Of<IUserClaimsPrincipalFactory<IdentityUser>>(),
        //    /* IOptions<IdentityOptions> optionsAccessor */null,
        //    /* ILogger<SignInManager<TUser>> logger */null,
        //    /* IAuthenticationSchemeProvider schemes */null,
        //    /* IUserConfirmation<TUser> confirmation */null);

        //var testSignInManager=new Mock<FakeSignInManager>();
        //_testSignInManager = new Mock<FakeSignInManager>();
        //_testSignInManager = new FakeSignInManager();

        _mediator = new Mock<IMediator>();
        //_identityService = new Mock<IIdentityService>(_userManager.Object, _testSignInManager.Object, _httpContextAccessor.Object);
        //_mapper = new Mock<IMapper>();
        //_identityService= new Mock<IIdentityService>(); 


        //_identityService = new Mock<IIdentityService>();
        // Build DbContextOptions
        dbContextOptions = new DbContextOptionsBuilder<OrienteeringContext>()
            .UseInMemoryDatabase(databaseName: "orienteeringTest")
            .Options;

        // "Mocker" automapper Fix bruker mock nå heller
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

    ////[Fact]

    ////public async Task CreateCheckpointTest()
    ////{
    ////    //var inMemoryTest = new InMemoryTest();
    ////    var _db = new OrienteeringContext(dbContextOptions, null);
    ////    if (!_db.Database.IsInMemory())
    ////    {
    ////        _db.Database.Migrate();
    ////    }


    ////    var trackId = new Guid();
    ////    Checkpoint checkpoint = new("test1", 0, trackId);
    ////    CheckpointDto checkpointDto = new("test1", trackId, 0);

    ////    var request = new CreateCheckpoint.Request(checkpointDto);
    ////    var handler = new CreateCheckpoint.Handler(_db);
    ////    var checkpointId = handler.Handle(request, CancellationToken.None).GetAwaiter().GetResult();
    ////    var result = await _db.Checkpoints.FirstOrDefaultAsync(c => c.Id == checkpointId);
    ////    Assert.Equal(checkpoint, result);
    ////}

    ////[Fact]

    ////public async Task GenerateQRTest()
    ////{
    ////    //var inMemoryTest = new InMemoryTest();
    ////    var _db = new OrienteeringContext(dbContextOptions, null);
    ////    if (!_db.Database.IsInMemory())
    ////    {
    ////        _db.Database.Migrate();
    ////    }


    ////    var trackId = new Guid();
    ////    Checkpoint checkpoint = new("test1", 0, trackId);
    ////    CheckpointDto checkpointDto = new("test1", trackId, 0);

    ////    var request = new CreateCheckpoint.Request(checkpointDto);
    ////    var handler = new CreateCheckpoint.Handler(_db);
    ////    var checkpointId = handler.Handle(request, CancellationToken.None).GetAwaiter().GetResult();
    ////    var result = await _db.Checkpoints.FirstOrDefaultAsync(c => c.Id == checkpointId);
    ////    Assert.Equal(checkpoint, result);
    ////}



    ////fix denne testen under skal med
    ////[Fact]
    ////public async Task GetCheckpointForTracksTest()
    ////{
    ////    //Arrange
    ////    var mediator = new Mock<IMediator>();

    ////    var _db = new OrienteeringContext(dbContextOptions, null);
    ////    if (!_db.Database.IsInMemory())
    ////    {
    ////        _db.Database.Migrate();
    ////    }

    ////    var trackId = Guid.NewGuid();

    ////    var checkpoint1 = new Checkpoint("test1", 0, trackId);
    ////    var checkpoint2 = new Checkpoint("test2", 0, trackId);

    ////    await _db.Checkpoints.AddAsync(checkpoint1);
    ////    await _db.Checkpoints.AddAsync(checkpoint2);
    ////    await _db.SaveChangesAsync();
    ////    var request = new GetCheckpointsForTrack.Request(trackId);
    ////    var handler = new GetCheckpointsForTrack.Handler(_db, _mapper, mediator.Object);
    ////    var returnedDtoList = handler.Handle(request, CancellationToken.None).GetAwaiter().GetResult();
    ////    var checkpoint1Dto = _mapper.Map<Checkpoint, CheckpointDto>(checkpoint1);
    ////    var checkpoint2Dto = _mapper.Map<Checkpoint, CheckpointDto>(checkpoint2);
    ////    Assert.Equal(JsonConvert.SerializeObject(checkpoint1Dto), JsonConvert.SerializeObject(returnedDtoList[0]));
    ////    Assert.Equal(JsonConvert.SerializeObject(checkpoint2Dto), JsonConvert.SerializeObject(returnedDtoList[1]));
    ////}

    ////[Fact]
    ////public async Task GetQRCodesTest()
    ////{
    ////    var _db = new OrienteeringContext(dbContextOptions, null);
    ////    if (!_db.Database.IsInMemory())
    ////    {
    ////        _db.Database.Migrate();
    ////    }

    ////    var trackId = Guid.NewGuid();

    ////    var checkpoint1 = new Checkpoint("test1", 0, trackId);
    ////    checkpoint1.QRCode = "dGVzdD" 
    ////    var checkpoint2 = new Checkpoint("test2", 0, trackId);

    ////    await _db.Checkpoints.AddAsync(checkpoint1);
    ////    await _db.Checkpoints.AddAsync(checkpoint2);
    ////    await _db.SaveChangesAsync();
    ////    var request = new GetCheckpointsForTrack.Request(trackId);
    ////    var handler = new GetCheckpointsForTrack.Handler(_db, _mapper);
    ////    var returnedDtoList = handler.Handle(request, CancellationToken.None).GetAwaiter().GetResult();
    ////    var checkpoint1Dto = _mapper.Map<Checkpoint, CheckpointDto>(checkpoint1);
    ////    var checkpoint2Dto = _mapper.Map<Checkpoint, CheckpointDto>(checkpoint2);
    ////    Assert.Equal(JsonConvert.SerializeObject(checkpoint1Dto), JsonConvert.SerializeObject(returnedDtoList[0]));
    ////    Assert.Equal(JsonConvert.SerializeObject(checkpoint2Dto), JsonConvert.SerializeObject(returnedDtoList[1]));
    ////}

    //[Fact]

    //public async Task GetSingleCheckpointTest()
    //{
    //    //var mockHttpContextAccessor = new Mock<IHttpContextAccessor>();
    //    //mockHttpContextAccessor.Setup(x => x.HttpContext);


    //    //fix-pass på at bruker er logget in



    //    //se på denne?
    //    //https://mazeez.dev/posts/auth-in-integration-tests

    //    //var inMemoryTest = new InMemoryTest();
    //    var _db = new OrienteeringContext(dbContextOptions, null);
    //    if (!_db.Database.IsInMemory())
    //    {
    //        _db.Database.Migrate();
    //    }


    //    var trackId = Guid.NewGuid();
    //    var checkpoint = new Checkpoint("test1", 0, trackId);
    //    CheckpointDto checkpointDto = new("test1", trackId, 0);
    //    await _db.Checkpoints.AddAsync(checkpoint);
    //    await _db.SaveChangesAsync();
    //    var request = new GetSingleCheckpoint.Request(checkpoint.Id);
    //    var handler = new GetSingleCheckpoint.Handler(_db, _mapper.Object, _identityService.Object, _mediator.Object);
    //    var returnedDto = handler.Handle(request, CancellationToken.None).GetAwaiter().GetResult();
    //    Assert.Equal(checkpointDto.Title, returnedDto.Title);
    //    Assert.Equal(checkpointDto.TrackId, returnedDto.TrackId);
    //    Assert.Equal(checkpointDto.GameId, returnedDto.GameId);

    //}
    ////private static Mock<IHttpContextAccessor> GetHttpContextAccessor()
    ////{
    ////    var httpContextAccessorMock = new Mock<IHttpContextAccessor>();
    ////    httpContextAccessorMock.Setup(h => h.HttpContext.User).Returns(user);
    ////    return httpContextAccessorMock;
    ////}

    //[Fact]
    //public async Task AddQuizQuestionTest()
    //{
    //    ////arrange
    //    var _db = new OrienteeringContext(dbContextOptions, null);
    //    if (!_db.Database.IsInMemory())
    //    {
    //        _db.Database.Migrate();
    //    };

    //    //Mock IHttpContextAccessor
    //    //var mockHttpContextAccessor = new Mock<IHttpContextAccessor>();
    //    //var context = new DefaultHttpContext();
    //    //context.User.Claim
    //    //    string testid = Guid.NewGuid().ToString();
    //    //    var identity = new GenericIdentity("id", testid);

    //    //    var contextUser = new ClaimsPrincipal(identity); //add claims as needed
    //    //                                                     //...then set user and other required properties on the httpContext as needed
    //    //    var httpContext = new DefaultHttpContext()
    //    //    {
    //    //        User = contextUser;
    //    //}    //    string testid = Guid.NewGuid().ToString();
    //    //    var identity = new GenericIdentity("id", testid);

    //    //    var contextUser = new ClaimsPrincipal(identity); //add claims as needed
    //    //                                                     //...then set user and other required properties on the httpContext as needed
    //    //    var httpContext = new DefaultHttpContext()
    //    //    {
    //    //        User = contextUser;
    //    //}
    //    //https://gibinfrancis.medium.com/unit-test-quick-book-c-net-core-moq-xunit-f89370af84b8
    //    //private static ClaimsPrincipal user = new ClaimsPrincipal(
    //    //                new ClaimsIdentity(
    //    //                    new Claim[] { new Claim("MyClaim", "MyClaimValue") },
    //    //                    "Basic")
    //    //                );


    //    var quizIdString = System.Guid.NewGuid().ToString();
    //    var quizIdGuid = new Guid(quizIdString);
    //    var quiz = new Quiz(quizIdGuid);
    //    var quizQuestion = new QuizQuestion();
    //    quizQuestion.Question = "question";
    //    quizQuestion.CorrectAlternative = 1;
    //    var alt1 = new Alternative(1, "green");
    //    var alt2 = new Alternative(2, "red");
    //    quizQuestion.Alternatives.Add(alt1);
    //    quizQuestion.Alternatives.Add(alt2);
    //    var alternativeDtoList = new List<AlternativeDto>();
    //    alternativeDtoList.Add(_mapper.Object.Map<Alternative, AlternativeDto>(alt1));
    //    alternativeDtoList.Add(_mapper.Object.Map<Alternative, AlternativeDto>(alt2));
    //    await _db.Quiz.AddAsync(quiz);
    //    await _db.SaveChangesAsync();
    //    var inputCreateQuestionDto = new InputCreateQuestionDto(quizQuestion.Question, alternativeDtoList, quizQuestion.CorrectAlternative, quizIdString);
    //    quiz.QuizQuestions.Add(quizQuestion);
    //    var request = new AddQuizQuestion.Request(inputCreateQuestionDto);
    //    var handler = new AddQuizQuestion.Handler(_db, _identityService.Object, _mediator.Object);
    //    var result = handler.Handle(request, CancellationToken.None).GetAwaiter().GetResult();
    //    Assert.True(result);
    //    var addedQuiz = await _db.Quiz.Include(a => a.QuizQuestions).ThenInclude(b => b.Alternatives).FirstOrDefaultAsync(q => q.Id == quizIdGuid);
    //    Assert.Equal(JsonConvert.SerializeObject(quiz), JsonConvert.SerializeObject(addedQuiz));
    //}

    //[Fact]
    //public async Task GetNextQuizQuestionTest()
    //{
    //    var _db = new OrienteeringContext(dbContextOptions, null);
    //    if (!_db.Database.IsInMemory())
    //    {
    //        _db.Database.Migrate();
    //    }

    //    var quizId = Guid.NewGuid();
    //    var quiz = new Quiz(quizId);
    //    var quizQuestion = new QuizQuestion();
    //    quizQuestion.Question = "question";
    //    quizQuestion.CorrectAlternative = 1;
    //    var alt1 = new Alternative(1, "green");
    //    var alt2 = new Alternative(1, "red");
    //    quizQuestion.Alternatives.Add(alt1);
    //    quizQuestion.Alternatives.Add(alt2);
    //    quiz.QuizQuestions.Add(quizQuestion);

    //    await _db.Quiz.AddAsync(quiz);
    //    await _db.SaveChangesAsync();
    //    var request = new GetNextQuizQuestion.Request(quiz.Id, 0);
    //    var handler = new GetNextQuizQuestion.Handler(_db, _mapper.Object);
    //    var returnedDto = handler.Handle(request, CancellationToken.None).GetAwaiter().GetResult();
    //    //var nextQuizQuestion = _mapper.Map<QuizQuestion, NextQuizQuestionDto>(quizQuestion);

    //    var nextQuizQuestion = new NextQuizQuestionDto();
    //    var alternativeDtoList = new List<AlternativeDto>();
    //    var alternative1Dto = _mapper.Object.Map<Alternative, AlternativeDto>(alt1);
    //    var alternative2Dto = _mapper.Object.Map<Alternative, AlternativeDto>(alt2);
    //    alternativeDtoList.Add(alternative1Dto);
    //    alternativeDtoList.Add(alternative2Dto);
    //    nextQuizQuestion.Alternatives = alternativeDtoList;
    //    nextQuizQuestion.QuizQuestionId = quizQuestion.Id;
    //    nextQuizQuestion.Question = quizQuestion.Question;

    //    nextQuizQuestion.EndOfQuiz = true;
    //    Assert.Equal(JsonConvert.SerializeObject(returnedDto), JsonConvert.SerializeObject(nextQuizQuestion));
    //}

    //[Fact]
    //public async Task GetQuizTest()
    //{
    //    var _db = new OrienteeringContext(dbContextOptions, null);
    //    if (!_db.Database.IsInMemory())
    //    {
    //        _db.Database.Migrate();
    //    }
    //    var quizId = Guid.NewGuid();
    //    var quiz = new Quiz(quizId);
    //    var quizQuestion = new QuizQuestion();
    //    quizQuestion.Question = "question";
    //    quizQuestion.CorrectAlternative = 1;
    //    var alt1 = new Alternative(1, "green");
    //    var alt2 = new Alternative(2, "red");
    //    quizQuestion.Alternatives.Add(alt1);
    //    quizQuestion.Alternatives.Add(alt2);
    //    quiz.QuizQuestions.Add(quizQuestion);
    //    await _db.Quiz.AddAsync(quiz);
    //    await _db.SaveChangesAsync();
    //    var request = new GetQuiz.Request(quizId);
    //    var handler = new GetQuiz.Handler(_db, _mapper.Object);
    //    var returnedQuizDto = handler.Handle(request, CancellationToken.None).GetAwaiter().GetResult();

    //    var quizQuestionDto = new QuizQuestionDto();
    //    var alternativeDtoList = new List<AlternativeDto>();
    //    alternativeDtoList.Add(_mapper.Object.Map<Alternative, AlternativeDto>(alt1));
    //    alternativeDtoList.Add(_mapper.Object.Map<Alternative, AlternativeDto>(alt2));
    //    quizQuestionDto.Alternatives = alternativeDtoList;
    //    quizQuestionDto.QuizQuestionId = quizQuestion.Id;
    //    quizQuestionDto.Question = quizQuestion.Question;
    //    quizQuestionDto.CorrectAlternative = quizQuestion.CorrectAlternative;

    //    var quizDto = new QuizDto(quizId, new List<QuizQuestionDto> { quizQuestionDto });

    //    Assert.Equal(JsonConvert.SerializeObject(returnedQuizDto), JsonConvert.SerializeObject(quizDto));
    //}

    //[Fact]
    //public async Task GetSolutionTest()
    //{
    //    var _db = new OrienteeringContext(dbContextOptions, null);
    //    if (!_db.Database.IsInMemory())
    //    {
    //        _db.Database.Migrate();
    //    }
    //    var quizId = Guid.NewGuid();
    //    var quiz = new Quiz(quizId);
    //    var quizQuestion = new QuizQuestion();
    //    quizQuestion.Question = "question";
    //    quizQuestion.CorrectAlternative = 1;
    //    var alt1 = new Alternative(1, "green");
    //    var alt2 = new Alternative(2, "red");
    //    quizQuestion.Alternatives.Add(alt1);
    //    quizQuestion.Alternatives.Add(alt2);
    //    quiz.QuizQuestions.Add(quizQuestion);
    //    await _db.Quiz.AddAsync(quiz);
    //    await _db.SaveChangesAsync();
    //    var request = new GetSolution.Request(quizId, quizQuestion.Id);
    //    var handler = new GetSolution.Handler(_db);
    //    var result = handler.Handle(request, CancellationToken.None).GetAwaiter().GetResult();

    //    Assert.Equal("green", result);
    //}



    //[Fact]
    //public async Task CreateTrackTest()
    //{


    //    Assert.NotNull(3);
    //    var k = 2;
    //    var identityService = _identityService.Object;
    //    var t = await identityService.CreateUser(new UserRegistration("hei", "password123", "mail@gmail.com"));
    //    var a = await identityService.SignInUser(new UserSignIn("hei", "password123"));
    //    var h = identityService.GetCurrentUserId();
    //    //Assert.NotNull(null);
    //    var _db = new OrienteeringContext(dbContextOptions, null);
    //    if (!_db.Database.IsInMemory())
    //    {
    //        _db.Database.Migrate();
    //    }
    //    var trackDto = new CreateTrackDto("test");
    //    //trackDto.UserId = Guid.NewGuid();
    //    //trackDto.TrackName = "Test";
    //    //trackDto.TrackId = Guid.NewGuid();
    //    var request = new CreateTrack.Request(trackDto);
    //    var handler = new CreateTrack.Handler(_db, _mapper.Object, identityService);
    //    var result = handler.Handle(request, CancellationToken.None).GetAwaiter().GetResult();
    //    //Assert.Equal(result, trackDto.TrackId);

    //    //var addedTrack = await _db.Tracks.FirstOrDefaultAsync(t => t.Id == trackDto.TrackId);
    //    var addedTrack = await _db.Tracks.FirstOrDefaultAsync(t => t.Name == "test");
    //    Assert.NotNull(addedTrack);
    //    var addedTrackDto = _mapper.Object.Map<Track, TrackDto>(addedTrack);
    //    Assert.Equal(JsonConvert.SerializeObject(trackDto), JsonConvert.SerializeObject(addedTrackDto));
    //}




    




    
    //[Fact]
    //public async Task GetTracksTest()
    //{
    //    var _db = new OrienteeringContext(dbContextOptions, null);
    //    if (!_db.Database.IsInMemory())
    //    {
    //        _db.Database.Migrate();
    //    }
    //    var userId = Guid.NewGuid();
    //    var track1 = new Track();
    //    track1.UserId = userId;
    //    track1.Name = "Test1";
    //    var track2 = new Track();
    //    track2.UserId = userId;
    //    track2.Name = "Test2";
    //    await _db.Tracks.AddAsync(track1);
    //    await _db.Tracks.AddAsync(track2);
    //    await _db.SaveChangesAsync();
    //    var request = new GetTracks.Request();
    //    var handler = new GetTracks.Handler(_db, _mapper.Object, _identityService.Object);
    //    var returnedTrackDtoList = handler.Handle(request, CancellationToken.None).GetAwaiter().GetResult();
    //    var track1Dto = _mapper.Object.Map<Track, TrackDto>(track1);
    //    var track2Dto = _mapper.Object.Map<Track, TrackDto>(track2);
    //    var tracDtoList = new List<TrackDto> { track1Dto, track2Dto };
    //    Assert.Equal(JsonConvert.SerializeObject(tracDtoList), JsonConvert.SerializeObject(returnedTrackDtoList));
    //}



    //[Fact]
    //public void MediatorServiceTest()
    //{


    //    //using var scope = factory.Server.Services.CreateScope();
    //    //var identityService = scope.ServiceProvider.GetService<IIdentityService>();

    //    //Assert.NotNull(identityService);
    //}


}
