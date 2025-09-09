using Asp.Versioning;
using Asp.Versioning.Builder;

namespace BookStore.Api.Endpoints;

public abstract class EndpointGroupBase : IEndpoint
{
    public static ApiVersionSet ApiVersionSet { get; private set; } = null!;
    public static RouteGroupBuilder RouteGroup { get; set; } = null!;
    public IEndpointRouteBuilder App { get; }

    protected EndpointGroupBase(IEndpointRouteBuilder app)
    {
        ApiVersionSet = app.NewApiVersionSet()
            .HasApiVersion(new ApiVersion(1, 0))
            .HasApiVersion(new ApiVersion(2, 0))
            .Build();

        RouteGroup = app
            .MapGroup("api/v{version:apiVersion}")
            .WithApiVersionSet(ApiVersionSet);

        App = app;
    }

    public abstract void MapEndpopint();
}
