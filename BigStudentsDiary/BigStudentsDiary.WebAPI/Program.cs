using BigStudentsDiary.Domain.Interfaces;
using BigStudentsDiary.Domain.Interfaces.Auth;
using BigStudentsDiary.Domain.Services;
using BigStudentsDiary.Infrastructure;
using BigStudentsDiary.WebAPI;
using BigStudentsDiary.WebAPI.EndPoints;
using Microsoft.AspNetCore.Authentication.JwtBearer;
using Microsoft.Extensions.Options;
using Microsoft.IdentityModel.Tokens;
using Serilog;
using System.Text;
using BigStudentsDiary.Domain.Interfaces.IRepositories;
using BigStudentsDiary.Infrastructure.Repositories;
using BigStudentsDiary.WebAPI.Extensions;
using Microsoft.AspNetCore.CookiePolicy;

var builder = WebApplication.CreateBuilder(args);

// Добавление логгера
Log.Logger = new LoggerConfiguration()
    .WriteTo.Console()
    .WriteTo.RollingFile("logs/test-{Date}.txt")
    .CreateLogger();



// Регистрация сервисов
builder.Services.AddControllers();
builder.Services.AddEndpointsApiExplorer();
builder.Services.AddSwaggerGen();

builder.Services.AddSingleton<IStudentsRepository, StudentsRepository>();
builder.Services.AddSingleton<ITeachersRepository, TeachersRepository>();
builder.Services.AddSingleton<IHomeWorksRepository, HomeWorksRepository>();
builder.Services.AddSingleton<IFloorRepository, FloorRepository>();
builder.Services.AddSingleton<ITimeTableRepository, TimeTableRepository>();
builder.Services.AddSingleton<IGroupRepository, GroupsRepository>();
builder.Services.AddSingleton<IDepartmentRepository, DepartmentRepository>();
builder.Services.AddSingleton<IDisciplinesRepository, DisciplinesRepository>();
builder.Services.AddSingleton<IBuildingRepository, BuildingRepository>();
builder.Services.AddSingleton<IRoomRepository, RoomRepository>();
builder.Services.AddSingleton<IJwtProvider, JwtProvider>();
builder.Services.AddSingleton<IPasswordHasher, PasswordHasher>(); 
builder.Services.AddSingleton<StudentsService>();
builder.Services.AddSingleton<TeachersService>();
builder.Services.AddSingleton<TimeTableService>();
builder.Services.Configure<JwtOptions>(builder.Configuration.GetSection("JwtOptions")); 

builder.Services.AddSingleton<ExceptionMiddleware>();

// Настройка аутентификации
builder.Services.AddApiAuthentication(builder.Configuration);

var app = builder.Build();

// Configure the HTTP request pipeline.
if (app.Environment.IsDevelopment())
{
    app.UseSwagger();
    app.UseSwaggerUI();
}

app.UseCors(options => options
    .WithOrigins("http://localhost:64959","http://localhost:8080") // Замените на точный порт, который используется Flutter-приложением
    .AllowAnyHeader()
    .AllowAnyMethod()
    .AllowCredentials());

app.UseHttpsRedirection();

app.UseCookiePolicy(new CookiePolicyOptions
{
    MinimumSameSitePolicy = SameSiteMode.Strict,
    HttpOnly = HttpOnlyPolicy.Always,
    Secure = CookieSecurePolicy.Always
});

app.UseAuthentication();
app.UseAuthorization();

app.UseMiddleware<ExceptionMiddleware>();

app.MapControllers();
app.MapStudentEndPoints();
app.MapTeacherEndPoints();

app.Run();