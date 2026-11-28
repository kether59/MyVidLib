using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Metadata.Builders;
using MyVidLib.Domain.Media;

namespace MyVidLib.Infrastructure.Persistence.Configurations;

/// <summary>
/// Configuration de l'entité MediaFile : unicité par racine et chemin normalisé,
/// index de disponibilité, empreinte unique.
/// </summary>
public class MediaFileConfiguration : IEntityTypeConfiguration<MediaFile>
{
    public void Configure(EntityTypeBuilder<MediaFile> builder)
    {
        builder.ToTable("MediaFiles");

        builder.HasKey(e => e.Id);

        builder.HasIndex(e => new { e.MediaRootId, e.NormalizedRelativePath })
            .HasDatabaseName("UQ_MediaFiles_Root_RelativePath")
            .IsUnique();

        builder.HasIndex(e => new { e.MediaRootId })
            .HasDatabaseName("IX_MediaFiles_MediaRootId");

        builder.HasIndex(e => e.MediaWorkId)
            .HasDatabaseName("IX_MediaFiles_MediaWorkId");

        builder.HasIndex(e => new { e.AvailabilityStatus })
            .HasDatabaseName("IX_MediaFiles_AvailabilityStatus");

        builder.HasIndex(e => e.SizeBytes)
            .HasDatabaseName("IX_MediaFiles_SizeBytes");

        builder.HasIndex(e => new { e.MediaRootId, e.LastVerifiedAt })
            .HasDatabaseName("IX_MediaFiles_LastVerifiedAt");
    }
}