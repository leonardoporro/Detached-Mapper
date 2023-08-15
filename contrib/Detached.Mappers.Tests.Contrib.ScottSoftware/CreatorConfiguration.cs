using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Metadata.Builders;

namespace Detached.Mappers.Tests.Contrib.ScottSoftware;

public class CreatorConfiguration : IEntityTypeConfiguration<Creator>
{
    public void Configure(EntityTypeBuilder<Creator> builder)
    {
        builder.HasKey(p => p.Id);
        builder.HasMany(p => p.Works).WithOne(a => a.Creator).OnDelete(DeleteBehavior.Cascade);
    }
}

