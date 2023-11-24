using LDInfo.Api.Features;
using LDInfo.Api.Features.Projects;
using LDInfo.Api.Features.Seeder;
using LDInfo.Api.Features.TimeLogs;
using LDInfo.Api.Features.Users;
using LDInfo.API.Utils;
using LDInfo.Data;
using Microsoft.EntityFrameworkCore;

var builder = WebApplication.CreateBuilder(args);

// Add services to the container.

builder.Services.AddControllers();
// Learn more about configuring Swagger/OpenAPI at https://aka.ms/aspnetcore/swashbuckle
builder.Services.AddEndpointsApiExplorer();
builder.Services.AddSwaggerGen();

builder.Services.AddDbContext<LDInfoDbContext>(options =>
    options.UseSqlServer(builder.Configuration.GetConnectionString(StaticConstants.CONNECTION_STRING)));

builder.Services
    .AddScoped<IUserService, UserService>()
    .AddScoped<ITimeLogService, TimeLogService>()
    .AddScoped<IProjectService, ProjectService>()
    .AddScoped<ISeederService, SeederService>();

builder.Services.AddAutoMapper(typeof(AutoMapperProfiles));

var app = builder.Build();

// Configure the HTTP request pipeline.
if (app.Environment.IsDevelopment())
{
    app.UseSwagger();
    app.UseSwaggerUI();
}

app.UseHttpsRedirection();

app.UseCors(options => options
        .AllowAnyOrigin()
        .AllowAnyMethod()
        .AllowAnyHeader());

app.UseAuthorization();

app.MapControllers();

app.Run();
