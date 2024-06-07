using BigStudentsDiary.Domain.Services;
using BigStudentsDiary.WebAPI.Contracts;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;

namespace BigStudentsDiary.WebAPI.EndPoints;

public static class StudentEndPoints
{
    public static IEndpointRouteBuilder MapStudentEndPoints(this IEndpointRouteBuilder app)
    {
        app.MapPost("/register", Register)
            .WithName("RegisterStudent")
            .WithTags("Student")
            .Produces(StatusCodes.Status200OK)
            .Produces(StatusCodes.Status400BadRequest);

        app.MapPost("/login", Login)
            .WithName("LoginStudent")
            .WithTags("Student")
            .Produces(StatusCodes.Status200OK)
            .Produces(StatusCodes.Status401Unauthorized);

        return app;
    }

    private static async Task<IResult> Register([FromBody] RegisterStudentRequest request, [FromServices] StudentsService studentsService)
    {
        await studentsService.Register(request.StudentName, request.StudentSurname, request.StudentLogin, request.StudentPassword, request.GroupId);
        return Results.Ok();
    }

    private static async Task<IResult> Login([FromBody] LoginStudentRequest request, [FromServices] StudentsService studentsService, HttpContext context)
    {
        var token = await studentsService.Login(request.StudentLogin, request.StudentPassword);
        
        context.Response.Cookies.Append("cookie-talk",token);
        if (token == null)
        {
            return Results.Unauthorized();
        }
        return Results.Ok(token);
    }
}