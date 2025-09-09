using BookStore.Models;
using BookStore.Models.Extentions;
using LanguageExt.Common;

namespace BookStore.Api.Infrastructure.Middlewares;

public class AuthFailureMiddleware
{
    private readonly RequestDelegate _next;

    public AuthFailureMiddleware(RequestDelegate next)
    {
        _next = next;
    }

    public async Task InvokeAsync(HttpContext context)
    {
        await _next(context);
        
        context.Response.ContentType = "application/json";

        if (context.Response.StatusCode == StatusCodes.Status400BadRequest)
        {
            //context.Response.
            //await SetResponse(
            //    context,
            //    context.Response.StatusCode == 401 ? "User Unauthorized" : "Access denied");
        }
        if (context.Response.StatusCode == StatusCodes.Status401Unauthorized ||
            context.Response.StatusCode == StatusCodes.Status403Forbidden)
        {
            await SetResponse(
                context,
                context.Response.StatusCode == 401 ? "User Unauthorized" : "Access denied");
        }

    }

    private static async Task SetResponse(HttpContext context, string v)
    {
        var error = Error.New(
            context.Response.StatusCode,
            v
        );

        // Reset response and write custom object
        context.Response.Clear();
        context.Response.StatusCode = error.Code;
        await context.Response.WriteAsJsonAsync(error);
    }
}
