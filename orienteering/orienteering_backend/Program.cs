using MediatR;
using Microsoft.EntityFrameworkCore;
using orienteering_backend.Infrastructure.Data;
using Microsoft.AspNetCore.Identity;
using Microsoft.AspNetCore;
using orienteering_backend.Core.Domain.Authentication;
using Microsoft.AspNetCore.Authentication.JwtBearer;
using Microsoft.IdentityModel.Tokens;
using Microsoft.AspNetCore.Authentication.Cookies;
using Microsoft.Extensions.Options;
using orienteering_backend.Core.Domain.Authentication.Services;

var builder = WebApplication.CreateBuilder(args);

// Add services to the container.
builder.Services.AddScoped<IJwtService, JwtService>();
builder.Services.AddScoped<IIdentityService, IdentityService>();


builder.Services.AddControllers();

// Learn more about configuring Swagger/OpenAPI at https://aka.ms/aspnetcore/swashbuckle
builder.Services.AddEndpointsApiExplorer();
builder.Services.AddSwaggerGen();

//fix finne current user
builder.Services.AddHttpContextAccessor();

//signinmanager fix
//builder.Services.AddAuthentication();
builder.Services.AddAuthentication(options =>
{
    options.DefaultAuthenticateScheme = IdentityConstants.ApplicationScheme;
    options.DefaultChallengeScheme = IdentityConstants.ApplicationScheme;
    options.DefaultSignInScheme = IdentityConstants.ExternalScheme;
})
.AddIdentityCookies();


//builder.Services.AddAuthentication(CookieAuthenticationDefaults.AuthenticationScheme)
//    .AddCookie();


//builder.Services
//    .AddAuthentication(JwtBearerDefaults.AuthenticationScheme)
    //.AddJwtBearer(options =>
    //{
    //    options.TokenValidationParameters = new TokenValidationParameters()
    //    {
    //        ValidateIssuer = true,
    //        ValidateAudience = true,
    //        ValidateLifetime = true,
    //        ValidateIssuerSigningKey = true,
    //        ValidAudience = builder.Configuration["Jwt:Audience"],  //la til builder. her
    //        ValidIssuer = builder.Configuration["Jwt:Issuer"],              //la til builder. her
    //        IssuerSigningKey = new SymmetricSecurityKey(
    //            Encoding.UTF8.GetBytes(builder.Configuration["Jwt:Key"])    //la til builder. her
    //        )
    //    };
    //});



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

//login fungerer. med cookie

//fra dat240 malin
builder.Services.ConfigureApplicationCookie(options =>
{
    // cookie settings
    options.Cookie.HttpOnly = true;
    options.ExpireTimeSpan = TimeSpan.FromMinutes(5);

    //fix- lag skikkelig url- blir videresendt hvis ikke
    options.LoginPath = "/signin";
    options.AccessDeniedPath = "/AccessDenied";
    options.SlidingExpiration = true;
});

////https://stackoverflow.com/questions/52379176/invalidoperationexception-no-authentication-handler-is-registered-for-the-schem
//builder.Services.AddAuthentication(CookieAuthenticationDefaults.AuthenticationScheme)
//            .AddCookie(o =>
//            {
//                o.Cookie.Name = "coockiename";// options.CookieName;
//                //o.Cookie.Domain = options.CookieDomain;
//                o.SlidingExpiration = true;
//                //o.ExpireTimeSpan = options.CookieLifetime;
//                //o.TicketDataFormat = ticketFormat;
//                //o.CookieManager = new CustomChunkingCookieManager();
//            });



var version = new MySqlServerVersion(new Version(8, 0, 28));
var connectionString = "server=localhost; port=3306; database=orienteering; user=root; password=passord123";
builder.Services.AddMediatR(typeof(Program));



builder.Services.AddDbContext<OrienteeringContext>(
    options => options.UseMySql(connectionString: connectionString, serverVersion: version)
);

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

//trengs??
//app.UseAuthentication();

app.MapControllers();

//app.MapDefaultControllerRoute();

app.Run();
