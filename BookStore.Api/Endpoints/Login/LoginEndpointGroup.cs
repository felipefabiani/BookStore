namespace BookStore.Api.Endpoints.Login;

public abstract class LoginRouteGroup : EndpointGroupBase
{
    protected LoginRouteGroup(IEndpointRouteBuilder app)
        : base(app)
    {
        RouteGroup = RouteGroup
            .MapGroup("login");
            //.AllowAnonymous();
    }
}
