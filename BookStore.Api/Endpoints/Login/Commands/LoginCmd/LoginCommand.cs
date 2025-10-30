using BookStore.Api.Infrastructure.Filters;
using BookStore.Models.Feature.Login;
using Microsoft.AspNetCore.Mvc;

namespace BookStore.Api.Endpoints.Login.Commands.LoginCmd;

public sealed class LoginCommandEndpoint(IEndpointRouteBuilder app) :
    LoginRouteGroup(app)
{
    public override void MapEndpopint()
    {
        RouteGroup.MapPost("/", async (UserLoginRequest r, [FromServices] ILoginService service, CancellationToken c) =>
        {
            var response = await service.Login(r, c);

            if (response.HasToken)
            {
                return Results.Ok(response);
            }
            return Results.BadRequest("User or password incorrect!");
        })
            .AddValidationFilter<UserLoginRequest>()
            .WithName("LoginV1")
            .MapToApiVersion(1);

        RouteGroup.MapPost("/", async (UserLoginRequest r, LinkGenerator linkGenerator, CancellationToken c) =>
        {
            await Task.Delay(1000, c);

            return Results.Ok(new UserLoginResponse
            {
                FullName = "Test User",
                UserClaims = new List<string> { "Admin", "User" },
                UserRoles = new List<string> { "Admin", "User" },
            });
        })
            .WithName("LoginV2")
            .MapToApiVersion(2);
    }

    //public override async Task HandleAsync(UserLoginRequest r, CancellationToken c)
    //{
    //    Response = await _service.Login(r, c);

    //    // await Task.Delay(10_000, c);

    //    if (!Response.HasToken)
    //    {
    //        ThrowError("User or password incorrect!");
    //    }

    //    await SendAsync(Response, cancellation: c);
    //}
}
