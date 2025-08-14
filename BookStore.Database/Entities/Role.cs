namespace BookStore.Database.Entities;

public class Role : Entity
{
    public string Name { get; set; } = string.Empty;
    public List<UserRole> UserRoles { get; set; } = [];

}

public class RoleEntityTypeConfiguration : IEntityTypeConfiguration<Role>
{
    public void Configure(EntityTypeBuilder<Role> builder)
    {
        builder.HasIndex(x => x.Name).IsUnique();

        builder
            .Property(x => x.Name)
            .IsUnicode(false)
            .IsRequired()
            .HasMaxLength(50);


        builder
            .HasMany(x => x.UserRoles)
            .WithOne(x => x.Role)
            .HasForeignKey(x => x.RoleId);
    }
}
