﻿namespace BookStore.Database.Entities
{
    public class Claim : Entity
    {
        public string Name { get; set; } = string.Empty;
        public string Value { get; set; } = string.Empty;
        public List<User> Users { get; set; } = [];
    }

    public class ClaimEntityTypeConfiguration : IEntityTypeConfiguration<Claim>
    {
        public void Configure(EntityTypeBuilder<Claim> builder)
        {
            builder.HasIndex(x => x.Name).IsUnique();

            builder
                .Property(x => x.Name)
                .IsUnicode(false)
                .IsRequired()
                .HasMaxLength(50);

            builder
                .Property(x => x.Value)
                .IsUnicode(false)
                .IsRequired()
                .HasMaxLength(5);
        }
    }
}