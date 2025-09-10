using BookStoreApp.Client.Shared.Templates.FormBases;
using FluentValidation;
using System.Reflection;

namespace BookStoreApp.Client.Infrastructure;
public static class ServiceCollectionExtensions
{
    public static void AddAllServices(this IServiceCollection services)
    {
        var assembly = Assembly.GetExecutingAssembly();

        var serviceTypes = assembly.GetTypes()
            .Where(t =>
                typeof(Service).IsAssignableFrom(t) &&
                t.IsClass && !t.IsAbstract);

        foreach (var implementationType in serviceTypes)
        {
            var interfaceType = implementationType.GetInterfaces()
                .FirstOrDefault(i => i != typeof(Service) && typeof(Service).IsAssignableFrom(i));

            if (interfaceType != null)
            {
                services.AddScoped(interfaceType, implementationType);
            }
        }
    }

    public static void AddFluentValidators(
        this IServiceCollection services,
        List<string> assemblyNames)
    {
        var validators = assemblyNames.GetAssemblies();

        foreach (var validator in validators)
        {
            var arg = validator.BaseType!.GetGenericArguments().First();
            var baseType = typeof(AbstractValidator<>).MakeGenericType(arg);
            services.AddSingleton(baseType, validator);
        }
    }
    public static List<Type> GetAssemblies(this List<string> assemblyNames)
    {
        var refs = Assembly
            .GetEntryAssembly()!
            .GetReferencedAssemblies()
            .Where(assembly => assemblyNames.Any(name => assembly.FullName.Contains(name)))
            .Select(assembly => Assembly.Load(assembly))
            .ToList();

        return GetValidators(refs);
    }

    public static List<Type> GetValidators(this List<Assembly> assemblyNames)
    {
        return assemblyNames
            .SelectMany(assembly => assembly
                .GetTypes()
                .Where(t =>
                    !t.IsAbstract &&
                    t.BaseType is not null &&
                    t.BaseType.IsGenericType &&
                    t.BaseType.GetGenericTypeDefinition() == typeof(AbstractValidator<>)))
            .ToList() ??
            [];
    }
}
