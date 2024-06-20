using BigStudentsDiary.Domain.Services;
using BigStudentsDiary.WebAPI.Contracts;
using Microsoft.AspNetCore.Mvc;

namespace BigStudentsDiary.WebAPI.EndPoints;

public static class TeacherEndPoints
{
    public static IEndpointRouteBuilder MapTeacherEndPoints(this IEndpointRouteBuilder app)
    {
        app.MapPost("/teacher/login", Login)
            .WithName("LoginTeacher")
            .WithTags("Teacher")
            .Produces(StatusCodes.Status200OK)
            .Produces(StatusCodes.Status401Unauthorized);

        return app;
    }
    
    private static async Task<IResult> Login([FromBody] LoginTeacherRequest request, [FromServices] TeachersService teachersService, HttpContext context)
    {
        var token = await teachersService.Login(request.TeacherLogin, request.TeacherPassword);
        
        context.Response.Cookies.Append("cookie-talk",token);
        if (token == null)
        {
            return Results.Unauthorized();
        }
        return Results.Ok(token);
    }
}