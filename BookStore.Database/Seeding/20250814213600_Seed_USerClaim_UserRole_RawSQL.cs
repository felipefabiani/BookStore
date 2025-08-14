using BookStore.Database.Infrastructure;

namespace BookStore.Database.Seeding;

public class _20250814213600_Seed_USerClaim_UserRole_RawSQL : ISeederEnv
{
    public SeedEnvironmentEnum SeedEnvironments =>
            SeedEnvironmentEnum.Dev |
            SeedEnvironmentEnum.UAT1 |
            SeedEnvironmentEnum.PreProd;

    public async Task SeedAsync(BookStoreContext context, SeedEnvironmentEnum seedEnvironment)
    {
        if (!SeedEnvironments.HasFlag(seedEnvironment))
        {
            return;
        }

        await SeedUserClaim(context);
    }

    private static async Task SeedUserClaim(BookStoreContext context)
    {
        await context.Database.ExecuteSqlRawAsync(
            """
            INSERT INTO UsersClaims (ClaimsId, UsersId)
                 VALUES (1, 1), (2, 1), (3, 1), (4, 1), (5, 1), (6, 1) ,(7, 1), (8, 1), (9, 1),
                        (1, 2), (2, 2), (3, 2), (4, 2), 
                        (5, 3), (6, 3), (7, 3), (8, 3),
                        (9, 4)
            """);
    }
}