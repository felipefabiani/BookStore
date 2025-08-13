using BookStore.Helper;
using BookStore.Helper.Extensions;
using Microsoft.EntityFrameworkCore.Design;
using System.Reflection;

namespace BookStore.Database.Context;

public abstract class BookStoreAbstractContext : DbContext
{
    protected BookStoreAbstractContext(DbContextOptions contextOptions)
        : base(contextOptions)
    {
    }

    // public DbSet<BookStoreTest> BookStoreTests { get; set; } = null!;
    public virtual DbSet<BookStores> BookStore { get; set; } = null!;
    public virtual DbSet<User> Users { get; set; } = null!;
    public virtual DbSet<Role> Roles { get; set; } = null!;
    public virtual DbSet<Claim> Claims { get; set; } = null!;
    public virtual DbSet<UserClaim> UserClaims { get; set; } = null!;
    public virtual DbSet<UserRole> UserRoles { get; set; } = null!;

    protected override void OnModelCreating(ModelBuilder modelBuilder)
    {
        modelBuilder.ApplyConfigurationsFromAssembly(Assembly.GetExecutingAssembly());
    }
}
public class BookStoreContextFactory : IDesignTimeDbContextFactory<BookStoreContext>
{
    public BookStoreContext CreateDbContext(string[] args)
    {
        var optionsBuilder = new DbContextOptionsBuilder<BookStoreContext>();
        var connectionString = Environment.GetEnvironmentVariable(BookStoreConstants.ConnectionString);

        optionsBuilder.UseSqlServer(connectionString, x =>
        {
            x.MigrationsAssembly(typeof(BookStoreContext).Assembly.FullName);
            x.EnableRetryOnFailure(2);
        });

        return new BookStoreContext(optionsBuilder.Options);
    }
}

public class BookStoreContext : BookStoreAbstractContext
{
    public BookStoreContext(DbContextOptions<BookStoreContext> options)
        : base(options)
    {
    }
    protected override void OnModelCreating(ModelBuilder modelBuilder)
    {
        modelBuilder.ApplyConfigurationsFromAssembly(typeof(BookStoreContext).Assembly);

        var roles = new List<Role>
    {
        new() { Id = 1, Name = "Admin" },
        new() { Id = 2, Name = "Author" },
        new() { Id = 3, Name = "User" }
    };

        var claims = new List<Claim>
    {
        new() { Id = 1, Name = "BookStore_Moderate", Value = "100" },
        new() { Id = 2, Name = "BookStore_Delete", Value = "101" },
        new() { Id = 3, Name = "BookStore_Get_Pending_List", Value = "102" },
        new() { Id = 4, Name = "BookStore_Update", Value = "103" },
        new() { Id = 5, Name = "Author_Update_Profile", Value = "104" },
        new() { Id = 6, Name = "Author_Get_Own_List", Value = "200" },
        new() { Id = 7, Name = "Author_Save_Own", Value = "201" },
        new() { Id = 8, Name = "Author_Update_Own_Profile", Value = "202" },
        new() { Id = 9, Name = "user_reads", Value = "301" }
    };

        modelBuilder.Entity<Role>().HasData(roles.ToArray());
        modelBuilder.Entity<Claim>().HasData(claims.ToArray());

        var staticBirthDate = new DateTimeOffset(1985, 1, 1, 0, 0, 0, TimeSpan.Zero);

        modelBuilder.Entity<User>().HasData(
            new User
            {
                Id = 1,
                FirstName = "Full",
                LastName = "Access",
                Email = "full.access@article.ie",
                DateOfBirth = staticBirthDate,
                Password = "hashed-password-1"
            },
            new User
            {
                Id = 2,
                FirstName = "Admin",
                LastName = "Test",
                Email = "admin.test@article.ie",
                DateOfBirth = staticBirthDate,
                Password = "hashed-password-2"
            },
            new User
            {
                Id = 3,
                FirstName = "Author",
                LastName = "Test",
                Email = "author.test@article.ie",
                DateOfBirth = staticBirthDate,
                Password = "hashed-password-3"
            },
            new User
            {
                Id = 4,
                FirstName = "User",
                LastName = "Test",
                Email = "user.test@article.ie",
                DateOfBirth = staticBirthDate,
                Password = "hashed-password-4"
            }
        );

        modelBuilder.Entity<UserRole>().HasData(
            new { UserId = 1, RoleId = 1 },
            new { UserId = 1, RoleId = 2 },
            new { UserId = 1, RoleId = 3 },
            new { UserId = 2, RoleId = 1 },
            new { UserId = 3, RoleId = 2 },
            new { UserId = 4, RoleId = 3 }
        );

        modelBuilder.Entity<UserClaim>().HasData(
            new { UserId = 1, ClaimId = 1 },
            new { UserId = 1, ClaimId = 2 },
            new { UserId = 1, ClaimId = 3 },
            new { UserId = 1, ClaimId = 4 },
            new { UserId = 1, ClaimId = 5 },
            new { UserId = 1, ClaimId = 6 },
            new { UserId = 1, ClaimId = 7 },
            new { UserId = 1, ClaimId = 8 },
            new { UserId = 1, ClaimId = 9 },
            new { UserId = 2, ClaimId = 1 },
            new { UserId = 2, ClaimId = 2 },
            new { UserId = 2, ClaimId = 3 },
            new { UserId = 2, ClaimId = 4 },
            new { UserId = 3, ClaimId = 5 },
            new { UserId = 3, ClaimId = 6 },
            new { UserId = 3, ClaimId = 7 },
            new { UserId = 3, ClaimId = 8 },
            new { UserId = 4, ClaimId = 9 }
        );
    }

}

public class BookStoreReadOnlyContext : BookStoreAbstractContext
{
    public BookStoreReadOnlyContext(DbContextOptions<BookStoreReadOnlyContext> options)
        : base(options)
    {
        ChangeTracker.QueryTrackingBehavior = QueryTrackingBehavior.NoTracking;
    }
}