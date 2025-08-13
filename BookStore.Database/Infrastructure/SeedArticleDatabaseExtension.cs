using static BCrypt.Net.BCrypt;
namespace BookStore.Database.Infrastructure;

public static class SeedBookStoreDatabaseExtension
{
    public static string GetPassword(string pwd = "123456") =>
        EnhancedHashPassword(pwd, 12);

    public static async ValueTask Seed(this BookStoreContext context)
    {
        await context.SeedUsers();
    }

    public static async ValueTask SeedUsers(this BookStoreContext context)
    {
        if (await context.Users.AnyAsync())
            return;

        var roles = await context.Roles.ToListAsync();
        var claims = await context.Claims.ToListAsync();

        var users = new List<User>
        {
            new() {
                FirstName = "Full",
                LastName = "Access",
                Email = "full.access@article.ie",
                DateOfBirth = DateTimeOffset.Now.AddYears(-40),
                Password = GetPassword()
            },
            new() {
                FirstName = "Admin",
                LastName = "Test",
                Email = "admin.test@article.ie",
                DateOfBirth = DateTimeOffset.Now.AddYears(-40),
                Password = GetPassword()
            },
            new() {
                FirstName = "Author",
                LastName = "Test",
                Email = "author.test@article.ie",
                DateOfBirth = DateTimeOffset.Now.AddYears(-40),
                Password = GetPassword()
            },
            new() {
                FirstName = "User",
                LastName = "Test",
                Email = "user.test@article.ie",
                DateOfBirth = DateTimeOffset.Now.AddYears(-40),
                Password = GetPassword()
            }
        };

        await context.Users.AddRangeAsync(users);
        await context.SaveChangesAsync(); // Save to generate User IDs
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