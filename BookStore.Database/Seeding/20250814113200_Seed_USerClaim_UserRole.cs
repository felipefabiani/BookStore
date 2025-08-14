//using BookStore.Database.Infrastructure;

//namespace BookStore.Database.Seeding;

//public class _20250814113200_Seed_USerClaim_UserRole : ISeeder
//{
//    public async Task SeedAsync(BookStoreContext context, SeedEnvironmentEnum seedEnvironment)
//    {
//        var seedEnvs = 
//            SeedEnvironmentEnum.Dev & 
//            SeedEnvironmentEnum.UAT1 &
//            SeedEnvironmentEnum.PreProd;

//        if (!ISeeder.HasToSeedEnvironment(seedEnvs, seedEnvironment))
//        {
//            return;
//        }

//        await SeedUserRoles(context);
//        // await SeedUserClaim(context);

//        if (context.ChangeTracker.HasChanges())
//        {
//            await context.SaveChangesAsync();
//        }
//    }
//    private async Task SeedUserRoles(BookStoreContext context)
//    {
//        //var userRoles = new List<UserRole>
//        //{
//        //    new () { RoleId = 1, UserId = 1 },
//        //    new () { RoleId = 2, UserId = 1 },
//        //    new () { RoleId = 3, UserId = 1 },
//        //    //new () { RoleId = 1, UserId = 2 },
//        //    //new () { RoleId = 2, UserId = 3 },
//        //    //new () { RoleId = 3, UserId = 4 }
//        //};

//        //var nonExistingUserRoles = userRoles
//        //    .Where(ur => !context.UserRoles.Any(db => db.UserId == ur.UserId && db.RoleId == ur.RoleId))
//        //    .ToList();

//        // works but is inefficient:
//        //var existingClaims = await context.UserRoles
//        //    .Where(dbClaim => userRoles.Any(uc => uc.RoleId == dbClaim.RoleId && uc.UserId == dbClaim.UserId))
//        //    .ToListAsync();

//        //// Get existing RoleId–UserId pairs
//        //var existingPairs = await context.UserRoles
//        //    .Select(ur => new { ur.RoleId, ur.UserId })
//        //    .ToListAsync();

//        //var existingSet = new HashSet<(int RoleId, int UserId)>(
//        //    existingPairs.Select(p => (p.RoleId, p.UserId))
//        //);

//        //// Filter out already existing pairs
//        //var missingRoles = userRoles
//        //    .Where(ur => !existingSet.Contains((ur.RoleId, ur.UserId)))
//        //    .ToList();

//        // Add only missing ones
//        //if (userRoles.Count != 0)
//        //{
//        //    await context.UserRoles.AddRangeAsync(userRoles);
//        //}

//        foreach (var item in userRoles)
//        {
//            context.UserRoles.Add(item);
//            await context.SaveChangesAsync();
//        }
//    }


//    private async Task SeedUserClaim(BookStoreContext context)
//    {
//        var userClaims = new List<UserClaim>
//        {
//            new (){ ClaimId = 1, UserId = 1 },
//            new (){ ClaimId = 2, UserId = 1 },
//            //new (){ ClaimId = 3, UserId = 1 },
//            //new (){ ClaimId = 4, UserId = 1 },
//            //new (){ ClaimId = 5, UserId = 1 },
//            //new (){ ClaimId = 6, UserId = 1 },
//            //new (){ ClaimId = 7, UserId = 1 },
//            //new (){ ClaimId = 8, UserId = 1 },
//            //new (){ ClaimId = 9, UserId = 1 },

//            //new (){ ClaimId = 1, UserId = 2 },
//            //new (){ ClaimId = 2, UserId = 2 },
//            //new (){ ClaimId = 3, UserId = 2 },
//            //new (){ ClaimId = 4, UserId = 2 },

//            //new (){ ClaimId = 5, UserId = 3 },
//            //new (){ ClaimId = 6, UserId = 3 },
//            //new (){ ClaimId = 7, UserId = 3 },
//            //new (){ ClaimId = 8, UserId = 3 },

//            //new (){ ClaimId = 9, UserId = 4 }
//        };

//        // works but is inefficient:
//        //var existingClaims = await context.UserClaims
//        //    .Where(dbClaim => userClaims.Any(uc => uc.ClaimId == dbClaim.ClaimId && uc.UserId == dbClaim.UserId))
//        //    .ToListAsync();

//        // Step 1: Get existing ClaimId–UserId pairs from the database
//        var existingPairs = await context.UserClaims
//            .Select(uc => new { uc.ClaimId, uc.UserId })
//            .ToListAsync();

//        var existingSet = new HashSet<(int ClaimId, int UserId)>(
//            existingPairs.Select(p => (p.ClaimId, p.UserId))
//        );

//        // Step 2: Filter out already existing pairs
//        var missingClaims = userClaims
//            .Where(uc => !existingSet.Contains((uc.ClaimId, uc.UserId)))
//            .ToList();

//        // Step 3: Insert only the missing ones
//        if (missingClaims.Count != 0)
//        {
//            await context.UserClaims.AddRangeAsync(missingClaims);
//        }
//    }
//}