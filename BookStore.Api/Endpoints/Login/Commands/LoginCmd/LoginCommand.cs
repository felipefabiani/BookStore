using BookStore.Models.Feature.Login;

namespace BookStore.Api.Endpoints.Login.Commands.LoginCmd;

public sealed class LoginCommandEndpoint(IEndpointRouteBuilder app) :
    LoginRouteGroup(app)
    
{
    public override void MapEndpopint()
    {
        RouteGroup.MapPost("/", async (UserLoginRequest r, CancellationToken c) =>
        {
            // Simulate a delay for demonstration purposes
            await Task.Delay(1000, c);
            // Here you would typically call your service to handle the login logic
            // For now, we will just return a dummy response
            var response = new UserLoginResponse
            {
                // HasToken = true,
                // Token = "dummy-token"
            };
            if (!response.HasToken)
            {
                return Results.BadRequest(new Exception("User or password incorrect!"));
            }
            return Results.Ok(response);
        })
            .WithName("LoginV1")
            .MapToApiVersion(1);
        //    .WithTags("Authentication")
        //    .Produces<LoginResponse>(StatusCodes.Status200OK)
        //    .Produces<ErrorResponse>(StatusCodes.Status400BadRequest)
        //    .RequireAuthorization();;

        RouteGroup.MapPost("/", async (UserLoginRequest r, LinkGenerator linkGenerator, CancellationToken c) =>
        {
            await Task.Delay(1000, c);

            var urlLoginV1 = linkGenerator.GetPathByName("LoginV1", values: null);


            return Results.Ok(urlLoginV1);
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
