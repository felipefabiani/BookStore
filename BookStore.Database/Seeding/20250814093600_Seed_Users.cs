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

        //if (context.ChangeTracker.HasChanges())
        //{
        //    await context.SaveChangesAsync();
        //}
    }

    private async Task SeedUsers(BookStoreContext context)
    {
        var inputUsers = new List<User> {
            new() {
                FirstName = "Full",
                LastName = "Access",
                Email = "full.access@article.ie",
                DateOfBirth = DateTimeOffset.Now.AddYears(-40),
                Password = GetPassword(),
            },
            new() {
                FirstName = "Admin",
                LastName = "Test",
                Email = "admin.test@article.ie",
                DateOfBirth = DateTimeOffset.Now.AddYears(-40),
                Password = GetPassword(),
            },
            new() {
                FirstName = "Author",
                LastName = "Test",
                Email = "author.test@article.ie",
                DateOfBirth = DateTimeOffset.Now.AddYears(-40),
                Password = GetPassword(),
            },
            new() {
                FirstName = "User",
                LastName = "Test",
                Email = "user.test@article.ie",
                DateOfBirth = DateTimeOffset.Now.AddYears(-40),
                Password = GetPassword(),
            }
        };

        // Only add the missing users
        var missingUsers = inputUsers
            .Where(input => !context.Users.Any(dbUser =>
                dbUser.FirstName == input.FirstName &&
                dbUser.LastName == input.LastName &&
                dbUser.Email == input.Email))
            .ToList();

        await context.Users.AddRangeAsync(missingUsers);
    }
}