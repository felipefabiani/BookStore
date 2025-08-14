using BookStore.Database.Infrastructure;
using static BCrypt.Net.BCrypt;
namespace BookStore.Database.Seeding;

public class _20250814093600_Seed_Users : ISeeder
{
    private string GetPassword(string pwd = "123456") =>
        EnhancedHashPassword(pwd, 12);

    public async Task SeedAsync(BookStoreContext context, SeedEnvironmentEnum seedEnvironment)
    {
        var seedEnvs = 
            SeedEnvironmentEnum.Dev & 
            SeedEnvironmentEnum.UAT1 &
            SeedEnvironmentEnum.PreProd;

        if (!ISeeder.HasToSeedEnvironment(seedEnvs, seedEnvironment))
        {
            return;
        }

        await SeedUsers(context);

        if (context.ChangeTracker.HasChanges())
        {
            await context.SaveChangesAsync();
        }
    }

    private async Task SeedUsers(BookStoreContext context)
    {
        if (await context.Users.AnyAsync())
        {
            return; // Users already seeded
        }
        var roles = await context.Roles.ToListAsync();
        var claims = await context.Claims.ToListAsync();
        var users = new List<User>
        {
            new() {
                FirstName = "Full",
                LastName = "Access",
                Email = "full.access@article.ie",
                DateOfBirth = DateTimeOffset.Now.AddYears(-40),
                Password = GetPassword(),
                Roles = roles,
                Claims = claims
            },
            new() {
                FirstName = "Admin",
                LastName = "Test",
                Email = "admin.test@article.ie",
                DateOfBirth = DateTimeOffset.Now.AddYears(-40),
                Password = GetPassword(),
                Roles = roles.Where(x => x.Id == 1 || x.Id == 3).ToList(),
                Claims = claims.Where(x => x.Id <= 4).ToList()
            },
            new() {
                FirstName = "Author",
                LastName = "Test",
                Email = "author.test@article.ie",
                DateOfBirth = DateTimeOffset.Now.AddYears(-40),
                Password = GetPassword(),
                Roles = roles.Where(x => x.Id == 2 || x.Id == 3).ToList(),
                Claims = claims.Where(x => x.Id >= 5 && x.Id < 9).ToList()
                //UserRoles = [.. roles.Where(x => x.Id == 2 || x.Id == 3).Select(role => new UserRole { RoleId = role.Id })],
                //UserClaims = [.. claims.Where(x => x.Id >= 5 && x.Id < 9).Select(claim => new UserClaim { ClaimId = claim.Id })]
            },
            new() {
                FirstName = "User",
                LastName = "Test",
                Email = "user.test@article.ie",
                DateOfBirth = DateTimeOffset.Now.AddYears(-40),
                Password = GetPassword(),
                Roles = roles.Where(x => x.Id == 3).ToList(),
                Claims = claims.Where(x => x.Id >= 9 && x.Id <= 9).ToList()
                //UserRoles = [.. roles.Where(x => x.Id == 3).Select(role => new UserRole { RoleId = role.Id })],
                //UserClaims = [.. claims.Where(x => x.Id >= 9 && x.Id <= 9).Select(claim => new UserClaim { ClaimId = claim.Id })]
            }
        };
        await context.Users.AddRangeAsync(users);
    }
}