namespace BookStore.Database.Infrastructure;

public interface ISeeder
{
    Task SeedAsync(BookStoreContext context, SeedEnvironmentEnum env);

    [Obsolete("Use SeedEnvironments.HasFlag(seedEnvironment) instead.")]
    static bool HasToSeedEnvironment(SeedEnvironmentEnum seedEnvs, SeedEnvironmentEnum seedEnvironment) => seedEnvironment == (seedEnvs & seedEnvironment);
}

public interface ISeederEnv : ISeeder
{
    SeedEnvironmentEnum SeedEnvironments { get; }
}

[Flags]
public enum SeedEnvironmentEnum
{
    Dev = 1,
    UAT1 = 2,
    UAT2 = 4,
    UAT3 = 8,
    PreProd = 2048,
    Prod = 4096,
    All = Dev | UAT1 | UAT2 | UAT3 | PreProd | Prod
}