using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace BookStore.Database.Entities;
public class SeedEntity : Entity
{
    public string Name { get; set; } = string.Empty;
    public DateTimeOffset CreateOn { get; set; } = DateTimeOffset.UtcNow;
}

public class SeedEntityTypeConfiguration : IEntityTypeConfiguration<SeedEntity>
{
    public void Configure(EntityTypeBuilder<SeedEntity> builder)
    {
        builder.ToTable("__SeedHistory");
        builder.HasIndex(x => x.Name).IsUnique();

        builder
            .Property(x => x.Name)
            .IsUnicode(false)
            .IsRequired()
            .HasMaxLength(100);
    }
}