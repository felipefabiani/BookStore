using Microsoft.Extensions.Diagnostics.HealthChecks;
using System.Diagnostics;

namespace BookStore.AppHost;
internal static class ResourceBuilderExtensions
{
    internal static IResourceBuilder<T> WithSwaggerUi<T>(
        this IResourceBuilder<T> resource)
        where T : IResourceWithEndpoints
    {
        return resource.WithOpenApiUi(
            "swagger-ui-doc",
            "Swagger UI Documentation",
            string.Empty);
    }

    internal static IResourceBuilder<T> WithReDocUi<T>(
        this IResourceBuilder<T> resource)
        where T : IResourceWithEndpoints
    {
        return resource.WithOpenApiUi(
            "redoc-ui-doc",
            "Re Doc UI Documentation",
            "docs");
    }

    internal static IResourceBuilder<T> WithScalarUi<T>(
        this IResourceBuilder<T> resource)
        where T : IResourceWithEndpoints
    {
        return resource.WithOpenApiUi(
            "scalar-ui-doc",
            "Scalar UI Documentation",
            "scalar");
    }


    private static IResourceBuilder<T> WithOpenApiUi<T>(
        this IResourceBuilder<T> resource,
        string uiDoc,
        string uiName,
        string openApiUiPath)
        where T : IResourceWithEndpoints
    {
        return resource.WithCommand(
            uiDoc,
            uiName,
            executeCommand: _ =>
            {
                try
                {
                    var endpoint = resource.GetEndpoint("https");
                    var url = $"{endpoint.Url}/{openApiUiPath}";
                    Process.Start(new ProcessStartInfo(url) { UseShellExecute = true });

                    return Task.FromResult(new ExecuteCommandResult { Success = true });
                }
                catch (Exception e)
                {
                    return Task.FromResult(new ExecuteCommandResult { Success = false, ErrorMessage = e.ToString() });
                }
            },
            commandOptions: new CommandOptions
            {
                IconName = "Document",
                IconVariant = IconVariant.Filled,
                UpdateState = c => c.ResourceSnapshot.HealthStatus == HealthStatus.Healthy
                    ? ResourceCommandState.Enabled
                    : ResourceCommandState.Disabled,
            });
    }
}
