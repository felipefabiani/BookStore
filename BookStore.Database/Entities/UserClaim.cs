//namespace BookStore.Database.Entities;

//public class UserClaim
//{
//    public int UserId { get; set; }
//    public int ClaimId { get; set; }
//    public User User { get; set; } = new();
//    public Claim Claim { get; set; } = new();
//}

//public class UserClaimEntityTypeConfiguration : IEntityTypeConfiguration<UserClaim>
//{
//    public void Configure(EntityTypeBuilder<UserClaim> builder)
//    {        
//        //// Define composite primary key
//        builder.HasKey(x => new { x.UserId, x.ClaimId });


//        builder
//            .Property(x => x.UserId).ValueGeneratedNever(); // Ensure UserId is not auto-generated
//        builder
//            .Property(x => x.ClaimId).ValueGeneratedNever(); // Ensure UserId is not auto-generated


//        // Optional: Ensure uniqueness (redundant with PK but fine)
//        builder.HasIndex(x => new { x.UserId, x.ClaimId }).IsUnique();

//        //// Relationships(optional but good practice)
//        //builder.HasOne(x => x.User)
//        //       .WithMany(u => u.UserClaims)
//        //       .HasForeignKey(x => new { x.UserId, x.ClaimId });

//        //builder.HasOne(x => x.Claim)
//        //       .WithMany(c => c.UserClaims)
//        //       .HasForeignKey(x => x.ClaimId);
//    }
//}