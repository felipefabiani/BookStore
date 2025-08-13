using System.Reflection;

namespace BookStore.Database.Context;

public abstract class BookStoreAbstractContext : DbContext
{
    protected BookStoreAbstractContext(DbContextOptions contextOptions)
        : base(contextOptions)
    {
    }

    // public DbSet<ArticleTest> ArticleTests { get; set; } = null!;
    public virtual DbSet<Article> Articles { get; set; } = null!;
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
public class BookStoreContext : BookStoreAbstractContext
{
    public BookStoreContext(DbContextOptions<BookStoreContext> options)
        : base(options)
    {
    }
}

public class ArticleReadOnlyContext : BookStoreAbstractContext
{
    public ArticleReadOnlyContext(DbContextOptions<ArticleReadOnlyContext> options)
        : base(options)
    {
        ChangeTracker.QueryTrackingBehavior = QueryTrackingBehavior.NoTracking;
    }
}