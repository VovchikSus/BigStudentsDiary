using BigStudentsDiary.Domain.Interfaces;
using BigStudentsDiary.Infrastructure;
using BigStudentsDiary.WebAPI;
using Serilog;

var builder = WebApplication.CreateBuilder(args);

builder.Services.AddControllers();
// Learn more about configuring Swagger/OpenAPI at https://aka.ms/aspnetcore/swashbuckle
builder.Services.AddEndpointsApiExplorer();
builder.Services.AddSwaggerGen();
builder.Services.AddSingleton<IStudentsRepository, StudentsRepository>();
builder.Services.AddSingleton<ITeachersRepository, TeachersRepository>();
builder.Services.AddSingleton<ExceptionMiddleware>();

var app = builder.Build();
Log.Logger = new LoggerConfiguration()
    .WriteTo.Console()
    .WriteTo.RollingFile("test")
    .CreateLogger();


// Configure the HTTP request pipeline.
if (app.Environment.IsDevelopment())
{
    app.UseSwagger();
    app.UseSwaggerUI();
}

app.UseHttpsRedirection();

app.UseAuthorization();

app.UseMiddleware<ExceptionMiddleware>();

app.MapControllers();

app.Run();