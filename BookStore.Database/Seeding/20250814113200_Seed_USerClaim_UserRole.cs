using BookStore.Database.Entities;
using BookStore.Database.Infrastructure;

namespace BookStore.Database.Seeding;

public class _20250814113200_Seed_USerClaim_UserRole : ISeeder
{
    public async Task SeedAsync(BookStoreContext context, SeedEnvironmentEnum seedEnvironment)
    {
        var seedEnvs =
            SeedEnvironmentEnum.Dev |
            SeedEnvironmentEnum.UAT1 |
            SeedEnvironmentEnum.PreProd;

        if (!ISeeder.HasToSeedEnvironment(seedEnvs, seedEnvironment))
        {
            return;
        }

        await SeedUserRoles(context);
        // await SeedUserClaim(context);

        if (context.ChangeTracker.HasChanges())
        {
            await context.SaveChangesAsync();
        }
    }
    private static async Task SeedUserRoles(BookStoreContext context)
    {
        var dbRoles = await context.Roles.ToListAsync();
        var dbUsers = await context.Users.ToListAsync();

        await context.UserRoles.AddRangeAsync([.. dbUsers
            .Where(u => u.Id == 1)
            .SelectMany(u => dbRoles
                .Select(r => new UserRole { 
                    UserId = u.Id, 
                    RoleId = r.Id,
                    User = u,
                    Role = r
                })
            )]);

        await context.UserRoles.AddRangeAsync([.. dbUsers
            .Where(u => u.Id == 2)
            .SelectMany(u => dbRoles
                .Where(x => x.Id == 1 || x.Id == 3)
                .Select(r => new UserRole { 
                    UserId = u.Id, 
                    RoleId = r.Id,
                    User = u,
                    Role = r
                }))]);

        await context.UserRoles.AddRangeAsync([.. dbUsers
            .Where(u => u.Id == 3)
            .SelectMany(u => dbRoles
                .Where(x => x.Id == 2 || x.Id == 3)
                .Select(r => new UserRole { 
                    UserId = u.Id,
                    RoleId = r.Id,
                    User = u,
                    Role = r 
                }))]);

        await context.UserRoles.AddRangeAsync([.. dbUsers
            .Where(u => u.Id == 4)
            .SelectMany(u => dbRoles
                .Where(x => x.Id == 3)
                .Select(r => new UserRole { 
                    UserId = u.Id, 
                    RoleId = r.Id, 
                    User = u, 
                    Role = r 
                }))]);
    }
}