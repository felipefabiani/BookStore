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
    //public virtual DbSet<UserClaim> UserClaims { get; set; } = null!;
    //public virtual DbSet<UserRole> UserRoles { get; set; } = null!;

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

        modelBuilder.Entity<Role>().HasData([
            new() { Id = 1, Name = "Admin" },
            new() { Id = 2, Name = "Author" },
            new() { Id = 3, Name = "User" }
        ]);
        
        modelBuilder.Entity<Claim>().HasData([
            new() { Id = 1, Name = "BookStore_Moderate", Value = "100" },
            new() { Id = 2, Name = "BookStore_Delete", Value = "101" },
            new() { Id = 3, Name = "BookStore_Get_Pending_List", Value = "102" },
            new() { Id = 4, Name = "BookStore_Update", Value = "103" },
            new() { Id = 5, Name = "Author_Update_Profile", Value = "104" },
            new() { Id = 6, Name = "Author_Get_Own_List", Value = "200" },
            new() { Id = 7, Name = "Author_Save_Own", Value = "201" },
            new() { Id = 8, Name = "Author_Update_Own_Profile", Value = "202" },
            new() { Id = 9, Name = "user_reads", Value = "301" }
        ]);
    }
}

public class BookStoreReadOnlyContext : BookStoreAbstractContext
{
    public BookStoreReadOnlyContext(DbContextOptions<BookStoreReadOnlyContext> options)
        : base(options)
    {
        ChangeTracker.QueryTrackingBehavior = QueryTrackingBehavior.NoTracking;
    }
    protected override void OnModelCreating(ModelBuilder modelBuilder)
    {
        modelBuilder.ApplyConfigurationsFromAssembly(typeof(BookStoreReadOnlyContext).Assembly);
    }
}