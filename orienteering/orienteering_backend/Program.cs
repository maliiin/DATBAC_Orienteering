using MediatR;
using Microsoft.EntityFrameworkCore;
using orienteering_backend.Infrastructure.Data;
using Microsoft.AspNetCore.Identity;
using orienteering_backend.Core.Domain.Authentication.Services;

var builder = WebApplication.CreateBuilder(args);

// Add services to the container.
builder.Services.AddScoped<IIdentityService, IdentityService>();


builder.Services.AddControllers();

// Learn more about configuring Swagger/OpenAPI at https://aka.ms/aspnetcore/swashbuckle
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

//about the settings
//https://learn.microsoft.com/en-us/dotnet/api/microsoft.aspnetcore.identity.identityoptions?view=aspnetcore-6.0 //02.02.23
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
    //uten linjen under fant den ikke _signInManager i controlleren
    .AddSignInManager<SignInManager<IdentityUser>>()

    .AddEntityFrameworkStores<OrienteeringContext>();


//fra dat240 malin
builder.Services.ConfigureApplicationCookie(options =>
{
    // cookie settings
    options.Cookie.HttpOnly = true;
    //fiks fix sett ned tiden!!
    options.ExpireTimeSpan = TimeSpan.FromMinutes(35);

    //options.ExpireTimeSpan = TimeSpan.FromMinutes(5);

    //fix- lag skikkelig url- blir videresendt hvis ikke
    //options.LoginPath = "/login";
    //options.AccessDeniedPath = "/AccessDenied";
    options.SlidingExpiration = true;
});

var version = new MySqlServerVersion(new Version(8, 0, 28));
var connectionString = "server=localhost; port=3306; database=orienteering; user=root; password=passord123";
builder.Services.AddMediatR(typeof(Program));

builder.Services.AddDbContext<OrienteeringContext>(
    options => options.UseMySql(connectionString: connectionString, serverVersion: version)
);
builder.Services.AddAutoMapper(AppDomain.CurrentDomain.GetAssemblies());

//Kilder: https://stackoverflow.com/questions/31886779/asp-net-mvc-6-aspnet-session-errors-unable-to-resolve-service-for-type (21.03.2023)

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
    app.UseSwaggerUI();//c=>{c.SwaggerEndpoint("/swagger/v1/swagger.json", "Employee API V1"););
}

app.UseHttpsRedirection();

app.UseAuthentication();
app.UseAuthorization();
app.UseSession();
app.MapControllers();

app.Run();
