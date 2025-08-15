using Serilog;
using System.Reflection;

namespace BookStore.Database.Infrastructure;

public static class SeedBookStoreDatabaseExtension
{

    public static async Task Seed(this BookStoreContext context, SeedEnvironmentEnum seedEnvironment)
    {
        Log.Information("Starting database seeding for environment: {Environment}", seedEnvironment);

        await EnsureCreateAndSeedAsync(context);
        await RunAllSeedersAsync(context, seedEnvironment);

        Log.Information("Database seeding completed");
    }

    public static async Task RunAllSeedersAsync(BookStoreContext context, SeedEnvironmentEnum seedEnvironment)
    {
        Log.Information("Discovering seeders...");

        var seeders = Assembly.GetExecutingAssembly()
            .GetTypes()
            .Where(type => typeof(ISeeder).IsAssignableFrom(type) &&
                    !type.IsInterface &&
                    !type.IsAbstract)
            .OrderBy(t => t.Name)
            .Select(Activator.CreateInstance)
            .Cast<ISeeder>()
            .ToArray();

        Log.Information("Found {SeederCount} seeders", seeders.Length);

        var missingSeeders = seeders
            .Where(seeder => !context.SeedHistory.Any(s => s.Name == seeder.GetType().Name))
            .ToList();

        if (missingSeeders.Count == 0)
        {
            Log.Information("No missing seeders. Skipping seeding.");
            return;
        }

        foreach (var seeder in missingSeeders)
        {
            var seederName = seeder.GetType().Name;
            try
            {
                Log.Information("Running seeder: {SeederName}", seederName);

                await seeder.SeedAsync(context, seedEnvironment);

                Log.Information("Seeder {SeederName} completed", seederName);
                Log.Information("Adding seeder history for: {SeederName}", seederName);
                await context.SeedHistory.AddAsync(new SeedEntity
                {
                    Name = seederName,
                    CreateOn = DateTimeOffset.UtcNow
                });

                await context.SaveChangesAsync();
                Log.Information("Seeder history for {SeederName} added", seederName);
            }
            catch (Exception ex)
            {
                Log.Error(ex, "Error running seeder: {SeederName}", seederName);
            }
        }
    }
    public static async Task EnsureDropCreateAndSeedAsync(this BookStoreContext context)
    {
        await context.Database.EnsureDeletedAsync();
        await EnsureCreateAndSeedAsync(context);
    }
    public static async Task EnsureCreateAndSeedAsync(this BookStoreContext context)
    {
        Log.Information("Ensuring database is updated");
        await context.Database.MigrateAsync();
        Log.Information("Database migration completed");
    }
}