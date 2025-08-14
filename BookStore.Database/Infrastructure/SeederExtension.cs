using System.Collections.Generic;
using System.Reflection;

namespace BookStore.Database.Infrastructure;

public static class SeedBookStoreDatabaseExtension
{
    public static async ValueTask Seed(this BookStoreContext context, SeedEnvironmentEnum seedEnvironment)
    {
        await EnsureCreateAndSeedAsync(context);
        await RunAllSeedersAsync(context, seedEnvironment);
    }
    
    public static async Task RunAllSeedersAsync(BookStoreContext context, SeedEnvironmentEnum seedEnvironment)
    {
        var seederType = typeof(ISeeder);

        var seeders = Assembly.GetExecutingAssembly()
            .GetTypes()
            .Where(type => typeof(ISeeder).IsAssignableFrom(type) &&
                   !type.IsInterface &&
                   !type.IsAbstract)        
            .OrderBy(t => t.Name) // Ensure consistent order
            .Select(Activator.CreateInstance)
            .Cast<ISeeder>()
            .ToArray();

        var missingSeeders = seeders
            .Where(seeder => !context.SeedHistory.Any(s => s.Name == seeder.GetType().Name))
            .ToList();
        
        if (missingSeeders.Count == 0)
        {
            return;
        }
        
        foreach (var seeder in missingSeeders)
        {
            await seeder.SeedAsync(context, seedEnvironment);
            await context.SeedHistory.AddAsync(new SeedEntity
            {
                Name = seeder.GetType().Name,
                CreateOn = DateTimeOffset.UtcNow
            });

            await context.SaveChangesAsync();
        }
    }
    public static async Task EnsureDropCreateAndSeedAsync(this BookStoreContext context)
    {
        await context.Database.EnsureDeletedAsync();
        await EnsureCreateAndSeedAsync(context);
    }
    public static async Task EnsureCreateAndSeedAsync(this BookStoreContext context)
    {
        await context.Database.MigrateAsync();
    }
}