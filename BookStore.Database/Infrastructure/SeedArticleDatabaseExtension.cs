﻿using static BCrypt.Net.BCrypt;
namespace BookStore.Database.Infrastructure;

public static class SeedArticleDatabaseExtension
{
    private static string GetPassword(string pwd = "123456") =>
        EnhancedHashPassword(pwd, 12);

    public static async ValueTask Seed(this BookStoreContext context)
    {
        await context.SeedUsers();
    }
    public static async ValueTask SeedUsers(this BookStoreContext context)
    {
        if (await context.Users.AnyAsync())
        {
            return;
        }

        var roles = context.Roles.ToList();
        var claims = context.Claims.ToList();

        await context.AddRangeAsync(
            new User()
            {
                FirstName = "Full",
                LastName = "Access",
                Email = "full.access@article.ie",
                DateOfBirday = DateTimeOffset.Now.AddYears(-40),
                Password = GetPassword(),
                Roles = roles,
                Claims = claims,
            },
            new User()
            {
                FirstName = "Admin",
                LastName = "Test",
                Email = "admin.test@article.ie",
                DateOfBirday = DateTimeOffset.Now.AddYears(-40),
                Password = GetPassword(),
                Roles = roles.Where(x => x.Id == 1).ToList(),
                Claims = claims.Where(x => x.Id <= 4).ToList()
            },
            new User()
            {
                FirstName = "Author",
                LastName = "Test",
                Email = "author.test@article.ie",
                DateOfBirday = DateTimeOffset.Now.AddYears(-40),
                Password = GetPassword(),
                Roles = roles.Where(x => x.Id == 2).ToList(),
                Claims = claims.Where(x => x.Id >= 5 && x.Id < 9).ToList()
            },
            new User()
            {
                FirstName = "User",
                LastName = "Test",
                Email = "user.test@article.ie",
                DateOfBirday = DateTimeOffset.Now.AddYears(-40),
                Password = GetPassword(),
                Roles = roles.Where(x => x.Id == 3).ToList(),
                Claims = claims.Where(x => x.Id >= 9 && x.Id <= 9).ToList()
            });
        await context.SaveChangesAsync();
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