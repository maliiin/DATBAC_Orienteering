using MediatR;
using Microsoft.EntityFrameworkCore;
using orienteering_backend.Infrastructure.Data;


var builder = WebApplication.CreateBuilder(args);

// Add services to the container.



builder.Services.AddControllers();
// Learn more about configuring Swagger/OpenAPI at https://aka.ms/aspnetcore/swashbuckle
builder.Services.AddEndpointsApiExplorer();
builder.Services.AddSwaggerGen();


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

app.UseAuthorization();

app.MapControllers();

app.Run();
