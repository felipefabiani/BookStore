using BookStoreApp.Components.FormBases;
using System.Reflection;

namespace BookStoreApp.Client.Infrastructure;
public static class ServiceCollectionExtensions
{
    public static void AddAllServices(this IServiceCollection services)
    {
        var assembly = Assembly.GetExecutingAssembly();

        var serviceTypes = assembly.GetTypes()
            .Where(t => 
                typeof(IService).IsAssignableFrom(t) && 
                t.IsClass && !t.IsAbstract);

        foreach (var implementationType in serviceTypes)
        {
            var interfaceType = implementationType.GetInterfaces()
                .FirstOrDefault(i => i != typeof(IService) && typeof(IService).IsAssignableFrom(i));

            if (interfaceType != null)
            {
                services.AddScoped(interfaceType, implementationType);
            }
        }
    }
}
