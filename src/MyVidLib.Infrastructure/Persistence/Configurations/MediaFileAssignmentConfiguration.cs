using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Metadata.Builders;
using MyVidLib.Domain.Media;

namespace MyVidLib.Infrastructure.Persistence.Configurations;

/// <summary>
/// Configuration de MediaFileAssignment : unicité de l'association Primary par œuvre,
/// unicité de fichier principal actif par fichier.
/// </summary>
public class MediaFileAssignmentConfiguration : IEntityTypeConfiguration<MediaFileAssignment>
{
    public void Configure(EntityTypeBuilder<MediaFileAssignment> builder)
    {
        builder.ToTable("MediaFileAssignments");

        builder.HasKey(e => e.Id);

        builder.HasIndex(e => new { e.MediaWorkId, e.Role })
            .HasDatabaseName("IX_MediaFileAssignments_Work_Role")
            .IsUnique();

        builder.HasIndex(e => new { e.MediaFileId, e.Role })
            .HasDatabaseName("IX_MediaFileAssignments_File_Role")
            .IsUnique();

        builder.HasIndex(e => new { e.MediaWorkId, e.MediaFileId })
            .HasDatabaseName("IX_MediaFileAssignments_Work_File");
    }
}