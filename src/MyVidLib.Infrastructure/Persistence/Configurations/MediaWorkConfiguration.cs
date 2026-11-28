using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Metadata.Builders;
using MyVidLib.Domain.Media;

namespace MyVidLib.Infrastructure.Persistence.Configurations;

/// <summary>
/// Configuration de l'entité MediaWork en héritage TPH.
/// </summary>
public class MediaWorkConfiguration : IEntityTypeConfiguration<MediaWork>
{
    public void Configure(EntityTypeBuilder<MediaWork> builder)
    {
        // Table name and schema
        builder.ToTable("MediaWorks");

        // Primary key
        builder.HasKey(e => e.Id);

        // Discriminator
        builder.HasDiscriminator<WorkType>("WorkType");

        // Index on WorkType for common queries
        builder.HasIndex(e => e.WorkType)
            .HasDatabaseName("IX_MediaWorks_WorkType");
    }
}