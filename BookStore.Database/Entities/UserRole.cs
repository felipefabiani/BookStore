//using Microsoft.EntityFrameworkCore.Metadata.Conventions;

//namespace BookStore.Database.Entities;

//public class UserRole: Entity
//{
//    public int UserId { get; set; }
//    public int RoleId { get; set; }

//    public User User { get; set; } = new();
//    public Role Role { get; set; } = new();
//}
//public class UserRoleEntityTypeConfiguration : IEntityTypeConfiguration<UserRole>
//{
//    public void Configure(EntityTypeBuilder<UserRole> builder)
//    {
//        // Define composite primary key
//        builder.HasKey(x => x.Id);


//        //builder
//        //    .Property(x => x.UserId).ValueGeneratedNever(); // Ensure UserId is not auto-generated
//        //builder
//        //    .Property(x => x.RoleId).ValueGeneratedNever(); // Ensure UserId is not auto-generated

//        // Optional: Ensure uniqueness (redundant with PK but fine)
//        builder.HasIndex(x => new { x.UserId, x.RoleId }).IsUnique();

//        //// Relationships(optional but good practice)
//        //builder.HasOne(x => x.User)
//        //       .WithMany(u => u.UserRoles)
//        //       .HasForeignKey(x => new { x.UserId, x.RoleId });

//        //builder.HasOne(x => x.Role)
//        //       .WithMany(r => r.UserRoles)
//        //       .HasForeignKey(x => x.RoleId);
//    }
//}