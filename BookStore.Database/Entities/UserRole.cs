namespace BookStore.Database.Entities;

public class UserRole
{
    public int UserId { get; set; }
    public int RoleId { get; set; }

    public User User { get; set; } = new();
    public Role Role { get; set; } = new();
}
public class UserRoleEntityTypeConfiguration : IEntityTypeConfiguration<UserRole>
{
    public void Configure(EntityTypeBuilder<UserRole> builder)
    {
        // Define composite primary key
        builder.HasKey(x => new { x.UserId, x.RoleId });

        // Optional: Ensure uniqueness (redundant with PK but fine)
        builder.HasIndex(x => new { x.UserId, x.RoleId }).IsUnique();

        // Relationships (optional but good practice)
        builder.HasOne(x => x.User)
               .WithMany(u => u.UserRoles)
               .HasForeignKey(x => x.UserId);

        builder.HasOne(x => x.Role)
               .WithMany(r => r.UserRoles)
               .HasForeignKey(x => x.RoleId);
    }
}