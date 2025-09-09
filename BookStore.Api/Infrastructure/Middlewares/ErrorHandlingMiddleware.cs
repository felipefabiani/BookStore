using BookStore.Models;
using System.ComponentModel.DataAnnotations;

namespace BookStore.Api.Infrastructure.Middlewares;

public class ErrorHandlingMiddleware
{
    private readonly RequestDelegate _next;

    public ErrorHandlingMiddleware(RequestDelegate next)
    {
        _next = next;
    }

    public async Task InvokeAsync(HttpContext context)
    {
        try
        {
            await _next(context);

            if (context.Response.StatusCode is >= 200 and < 300)
            {
                return;
            }

            if (context.Response.StatusCode == StatusCodes.Status401Unauthorized ||
                context.Response.StatusCode == StatusCodes.Status403Forbidden)
            {
                await SetResponse(
                    context,
                    context.Response.StatusCode == 401 ? "User Unauthorized" : "Access denied");
            }
        }
        catch (Exception)
        {
            await SetResponse(context, "Unknow error.");
        }
    }

    private static async Task SetResponse([Required]HttpContext context, string msg)
    {
        ArgumentNullException.ThrowIfNull(msg);
        var error = new ErrorRequest
        {
            StatusCode = context.Response.StatusCode,
            Message = msg 
        };

        context.Response.Clear();
        context.Response.ContentType = "application/json";
        context.Response.StatusCode = error.StatusCode;
        await context.Response.WriteAsJsonAsync(error);
    }
}
