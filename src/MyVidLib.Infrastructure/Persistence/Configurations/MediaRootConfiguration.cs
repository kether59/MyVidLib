using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Metadata.Builders;
using MyVidLib.Domain.Media;

namespace MyVidLib.Infrastructure.Persistence.Configurations;

/// <summary>
/// Configuration de l'entité MediaRoot : état d'availability, chemin absolu, mode casse.
/// </summary>
public class MediaRootConfiguration : IEntityTypeConfiguration<MediaRoot>
{
    public void Configure(EntityTypeBuilder<MediaRoot> builder)
    {
        builder.ToTable("MediaRoots");

        builder.HasKey(e => e.Id);

        builder.HasIndex(e => e.AvailabilityStatus).IsUnique();

        builder.HasIndex(e => e.AbsolutePath)
            .HasDatabaseName("UQ_MediaRoots_AbsolutePath")
            .IsUnique();
    }
}