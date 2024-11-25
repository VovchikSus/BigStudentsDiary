using BigStudentsDiary.Domain.Services;
using BigStudentsDiary.WebAPI.Contracts;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;

namespace BigStudentsDiary.WebAPI.EndPoints;

public static class StudentEndPoints
{
    public static IEndpointRouteBuilder MapStudentEndPoints(this IEndpointRouteBuilder app)
    {
        app.MapPost("/student/register", Register)
            .WithName("RegisterStudent")
            .WithTags("Student")
            .Produces(StatusCodes.Status200OK)
            .Produces(StatusCodes.Status400BadRequest);

        app.MapPost("/student/login", Login)
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
    
        if (token == null)
        {
            return Results.Unauthorized();
        }

        context.Response.Cookies.Append("Authorization", token);
        return Results.Ok(token);
    }

}