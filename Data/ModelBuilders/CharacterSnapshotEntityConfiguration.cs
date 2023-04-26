using ArkLens.Snapshots;
using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Metadata.Builders;

namespace Artisan.Data.ModelBuilders;

public class CharacterSnapshotEntityConfiguration : IEntityTypeConfiguration<CharacterSnapshot>
{
    public void Configure(EntityTypeBuilder<CharacterSnapshot> builder)
    {
        builder.HasKey(c => c.Name);
    }
}