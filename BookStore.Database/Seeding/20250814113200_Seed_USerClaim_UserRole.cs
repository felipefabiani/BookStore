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
    private async Task SeedUserRoles(BookStoreContext context)
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
                })
        )]);

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


    //private async Task SeedUserClaim(BookStoreContext context)
    //{
    //    var userClaims = new List<UserClaim>
    //    {
    //        new (){ ClaimId = 1, UserId = 1 },
    //        new (){ ClaimId = 2, UserId = 1 },
    //        //new (){ ClaimId = 3, UserId = 1 },
    //        //new (){ ClaimId = 4, UserId = 1 },
    //        //new (){ ClaimId = 5, UserId = 1 },
    //        //new (){ ClaimId = 6, UserId = 1 },
    //        //new (){ ClaimId = 7, UserId = 1 },
    //        //new (){ ClaimId = 8, UserId = 1 },
    //        //new (){ ClaimId = 9, UserId = 1 },

    //        //new (){ ClaimId = 1, UserId = 2 },
    //        //new (){ ClaimId = 2, UserId = 2 },
    //        //new (){ ClaimId = 3, UserId = 2 },
    //        //new (){ ClaimId = 4, UserId = 2 },

    //        //new (){ ClaimId = 5, UserId = 3 },
    //        //new (){ ClaimId = 6, UserId = 3 },
    //        //new (){ ClaimId = 7, UserId = 3 },
    //        //new (){ ClaimId = 8, UserId = 3 },

    //        //new (){ ClaimId = 9, UserId = 4 }
    //    };

    //    // works but is inefficient:
    //    //var existingClaims = await context.UserClaims
    //    //    .Where(dbClaim => userClaims.Any(uc => uc.ClaimId == dbClaim.ClaimId && uc.UserId == dbClaim.UserId))
    //    //    .ToListAsync();

    //    // Step 1: Get existing ClaimId–UserId pairs from the database
    //    var existingPairs = await context.UserClaims
    //        .Select(uc => new { uc.ClaimId, uc.UserId })
    //        .ToListAsync();

    //    var existingSet = new HashSet<(int ClaimId, int UserId)>(
    //        existingPairs.Select(p => (p.ClaimId, p.UserId))
    //    );

    //    // Step 2: Filter out already existing pairs
    //    var missingClaims = userClaims
    //        .Where(uc => !existingSet.Contains((uc.ClaimId, uc.UserId)))
    //        .ToList();

    //    // Step 3: Insert only the missing ones
    //    if (missingClaims.Count != 0)
    //    {
    //        await context.UserClaims.AddRangeAsync(missingClaims);
    //    }
    //}
}