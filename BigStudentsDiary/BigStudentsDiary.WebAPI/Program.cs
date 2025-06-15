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
using Microsoft.OpenApi.Models;

var builder = WebApplication.CreateBuilder(args);

// Добавление логгера
Log.Logger = new LoggerConfiguration()
    .WriteTo.Console()
    .WriteTo.RollingFile("logs/test-{Date}.txt")
    .CreateLogger();


builder.Logging.ClearProviders();
builder.Logging.AddConsole();
builder.Logging.AddDebug();
builder.Logging.SetMinimumLevel(LogLevel.Debug);
// Регистрация сервисов
builder.Services.AddControllers();
builder.Services.AddEndpointsApiExplorer();


builder.Services.AddSwaggerGen();
builder.Services.AddSwaggerGen(c =>
{
    c.SwaggerDoc("v1", new OpenApiInfo { Title = "VK Callback API", Version = "v1" });
    
});
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
builder.Services.AddSingleton<INoteRepository, NoteRepository>();
builder.Services.AddSingleton<IJwtProvider, JwtProvider>();
builder.Services.AddSingleton<IPasswordHasher, PasswordHasher>(); 
builder.Services.AddSingleton<StudentsService>();
builder.Services.AddSingleton<NoteService>();
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
//app.UseHttpsRedirection();
app.UseRouting();
app.UseCors(options => options
    .WithOrigins("http://localhost:7049","http://localhost:8080","https://champagne-biographies-batman-forming.trycloudflare.com") // Замените на точный порт, который используется Flutter-приложением
    .AllowAnyHeader()
    .AllowAnyMethod()
    .AllowCredentials());
app.UseWebSockets();
app.UseAuthentication();
app.UseAuthorization();

app.UseMiddleware<ExceptionMiddleware>();

app.UseCookiePolicy(new CookiePolicyOptions
{
    MinimumSameSitePolicy = SameSiteMode.Strict,
    HttpOnly = HttpOnlyPolicy.Always,
    Secure = CookieSecurePolicy.Always
});

app.UseEndpoints(endpoints => endpoints.MapControllers());



app.MapControllers();
app.MapStudentEndPoints();
app.MapTeacherEndPoints();
app.MapGet("/", () => "VK Callback API is running!");
app.MapGet("/ping", () => "pong");
app.Run("http://*:7049");