using BookStore.Api.Endpoints;
using Microsoft.Extensions.DependencyInjection.Extensions;
using System.Reflection;

namespace BookStore.Api.Infrastructure;

public static class SetUpEndpoints
{
    /// Register all endpoints here
    //public static IServiceCollection AddEndpoints(this IServiceCollection services, Assembly assembly)
    //{
    //    var endpointTypes = assembly.DefinedTypes
    //        .Where(t => typeof(IEndpoint).IsAssignableFrom(t) && !t.IsInterface && !t.IsAbstract)
    //        .Select(type => ServiceDescriptor.Transient(typeof(IEndpoint), type))
    //        .ToArray();

    //    services.TryAddEnumerable(endpointTypes);

    //    return services;
    //}

    //public static IApplicationBuilder MapEndpoints(
    //    this WebApplication app,
    //    Assembly assembly,
    //    RouteGroupBuilder? routeGroupBuilder = null)
    //{
    //    // var endpoints = app.Services.GetRequiredService<IEnumerable<IEndpoint>>();
    //    var endpoints = assembly.DefinedTypes
    //        .Where(t => typeof(IEndpoint).IsAssignableFrom(t) && !t.IsInterface && !t.IsAbstract)
    //        .Select(type => ServiceDescriptor.Transient(typeof(IEndpoint), type))
    //        .ToArray();

    //    IEndpointRouteBuilder builder = routeGroupBuilder is null
    //        ? app
    //        : routeGroupBuilder;

    //    foreach (var ep in endpoints)
    //    {
    //        ep.MapEndpopint(app);
    //    }
    //    return app;
    //}

    public static void RegisterEndpoints(this IEndpointRouteBuilder app)
    {
        var endpointTypes = Assembly.GetExecutingAssembly()
            .GetTypes()
            .Where(t => typeof(IEndpoint).IsAssignableFrom(t) && !t.IsInterface && !t.IsAbstract);

        foreach (var type in endpointTypes)
        {
            var endpoint = (IEndpoint)Activator.CreateInstance(type, app)!;
            endpoint.MapEndpopint();
        }
    }
}