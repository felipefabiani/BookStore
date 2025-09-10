using BookStore.Models;
using FluentValidation;

namespace BookStore.Api.Infrastructure.Filters;

public class ValidationFilter<T> : IEndpointFilter where T : class
{
    private readonly IValidator<T> _validator;

    public ValidationFilter(IValidator<T> validator)
    {
        _validator = validator;
    }

    public async ValueTask<object?> InvokeAsync(EndpointFilterInvocationContext context, EndpointFilterDelegate next)
    {
        if (context.Arguments.FirstOrDefault(x => x?.GetType() == typeof(T)) is not T obj)
        {
            return Results.BadRequest(new ErrorRequest
            {
                StatusCode = 400,
                Message = "Invalid request payload."
            });
        }

        var validationResult = await _validator.ValidateAsync(obj);

        if (!validationResult.IsValid)
        {
            // return TypedResults.ValidationProblem(error);
            return Results.BadRequest(new ErrorRequest
            {
                StatusCode = 400,
                Message = "Invalid object",
                Errors = [.. validationResult.Errors.Select(x => x.ErrorMessage)]
            });
        }

        return await next(context);
    }
}

public static class ValidationFilterExtensions
{
    public static RouteHandlerBuilder AddValidationFilter<T>(this RouteHandlerBuilder builder)
        where T : class
    {
        return builder
            .AddEndpointFilter<ValidationFilter<T>>()
            .ProducesValidationProblem();
    }
}