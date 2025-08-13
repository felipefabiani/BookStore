using BookStore.Helper;
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
    //public DbSet<UserClaim> UserClaims { get; set; } = null!;
    //public DbSet<UserRole> UserRoles { get; set; } = null!;

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
}

public class BookStoreReadOnlyContext : BookStoreAbstractContext
{
    public BookStoreReadOnlyContext(DbContextOptions<BookStoreReadOnlyContext> options)
        : base(options)
    {
        ChangeTracker.QueryTrackingBehavior = QueryTrackingBehavior.NoTracking;
    }
}