using Microsoft.EntityFrameworkCore;
using DomainServices;
using Infrastructure.Utilities;
using UniversityAPP.Utilities;
using UniversityAPP.Dto;

var builder = WebApplication.CreateBuilder(args);

// Add services to the container.
builder.Services.AddControllers();
// Learn more about configuring Swagger/OpenAPI at https://aka.ms/aspnetcore/swashbuckle
builder.Services.AddEndpointsApiExplorer();

string ConnectionString = builder.Configuration.GetConnectionString("ConString") ?? "";
var Jwt = new JwtData();
builder.Configuration.GetSection("JWT").Bind(Jwt);

builder.Services.APPConfiguration();
builder.Services.RepositoryConfiguration(ConnectionString);
builder.Services.DomainConfiguration();

builder.Services.AddAuthentication(Jwt);

var app = builder.Build();

//app.UseMiddleware<RequestLoggingMiddleware>();

// Configure the HTTP request pipeline.
if (app.Environment.IsDevelopment())
{
    app.UseSwagger();
    app.UseSwaggerUI();
}


app.UseHttpsRedirection();

app.UseAuthentication();
app.UseAuthorization();

app.MapControllers();

app.Run();
