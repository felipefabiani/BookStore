namespace BookStore.Database.Infrastructure;

public interface ISeeder
{
    Task SeedAsync(BookStoreContext context, SeedEnvironmentEnum env);
    static bool HasToSeedEnvironment(SeedEnvironmentEnum seedEnvs, SeedEnvironmentEnum seedEnvironment) => seedEnvironment == (seedEnvs | seedEnvironment);
}

[Flags]
public enum SeedEnvironmentEnum
{
    Dev = 1,
    UAT1 = 2,
    UAT2 = 4,
    UAT3 = 8,
    PreProd = 16,
    Prod =2024
}