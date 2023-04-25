using MediatR;
using Microsoft.EntityFrameworkCore;
using orienteering_backend.Infrastructure.Data;
using Microsoft.AspNetCore.Identity;
using orienteering_backend.Core.Domain.Authentication.Services;
using orienteering_backend.Core.Domain.Track.Services;

// License MediatR (Apache 2.0): https://github.com/jbogard/MediatR/blob/master/LICENSE
// Licence Pomelo (MIT): https://github.com/PomeloFoundation/Pomelo.EntityFrameworkCore.MySql/blob/master/LICENSE
// License .NET EFCore (MIT) https://github.com/dotnet/efcore/blob/main/LICENSE.txt

var builder = WebApplication.CreateBuilder(args);

// Add services to the container.
builder.Services.AddTransient<IIdentityService, IdentityService>();
builder.Services.AddTransient<ISessionService, SessionService>();


builder.Services.AddControllers();

builder.Services.AddEndpointsApiExplorer();
builder.Services.AddSwaggerGen();

builder.Services.AddHttpContextAccessor();

builder.Services.AddAuthentication(options =>
{
    options.DefaultAuthenticateScheme = IdentityConstants.ApplicationScheme;
    options.DefaultChallengeScheme = IdentityConstants.ApplicationScheme;
    options.DefaultSignInScheme = IdentityConstants.ExternalScheme;
})
.AddIdentityCookies();

builder.Services.AddIdentityCore<IdentityUser>(options =>
{
    //user settings
    options.SignIn.RequireConfirmedAccount = false;
    options.User.RequireUniqueEmail = true;

    //password settings
    options.Password.RequireDigit = false;
    options.Password.RequiredLength = 6;
    options.Password.RequireNonAlphanumeric = false;
    options.Password.RequireUppercase = false;
    options.Password.RequireLowercase = false; 
    })
    .AddSignInManager<SignInManager<IdentityUser>>()
    .AddEntityFrameworkStores<OrienteeringContext>();


builder.Services.ConfigureApplicationCookie(options =>
{
    // cookie settings
    options.Cookie.HttpOnly = true;
    options.ExpireTimeSpan = TimeSpan.FromMinutes(30);

    options.SlidingExpiration = true;
});

var version = new MySqlServerVersion(new Version(8, 0, 28));
var connectionString = "server=localhost; port=3306; database=orienteering; user=root; password=passord123";
builder.Services.AddMediatR(typeof(Program));

builder.Services.AddDbContext<OrienteeringContext>(
    options => options.UseMySql(connectionString: connectionString, serverVersion: version)
);
builder.Services.AddAutoMapper(AppDomain.CurrentDomain.GetAssemblies());

builder.Services.AddDistributedMemoryCache();

builder.Services.AddSession(options =>
{
    options.IdleTimeout = TimeSpan.FromSeconds(1800);
    options.Cookie.HttpOnly = true;
    options.Cookie.IsEssential = true;
});

var app = builder.Build();

// Configure the HTTP request pipeline.
if (app.Environment.IsDevelopment())
{
    app.UseSwagger();
    app.UseSwaggerUI();
}

app.UseHttpsRedirection();

app.UseAuthentication();
app.UseAuthorization();
app.UseSession();
app.MapControllers();

app.Run();

public partial class Program { }