using Serilog;

namespace BigStudentsDiary.WebAPI;

public class ExceptionMiddleware : IMiddleware
{
    public async Task InvokeAsync(HttpContext context, RequestDelegate next)
    {
        if (context.Response == null)
            await next.Invoke(context);

        try
        {
            await next.Invoke(context);
        }
        catch (Exception ex)
        {
            Log.Logger.Error(ex, ex.Message);

            context.Response.ContentType = "application/json";
            context.Response.StatusCode = StatusCodes.Status500InternalServerError;
            await context.Response.WriteAsJsonAsync("Ашипка");
        }
    }
}