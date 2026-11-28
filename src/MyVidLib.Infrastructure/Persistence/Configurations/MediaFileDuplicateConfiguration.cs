using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Metadata.Builders;
using MyVidLib.Domain.Media;

/// <summary>
/// Configuration de MediaFileDuplicate : couples fichier principal / copie,
/// index d'unicité par œuvre et par fichier, et liens vers l'œuvre.
/// </summary>
public class MediaFileDuplicateConfiguration : IEntityTypeConfiguration<MediaFileDuplicate>
{
    public void Configure(EntityTypeBuilder<MediaFileDuplicate> builder)
    {
        builder.ToTable("MediaFileDuplicates");

        builder.HasKey(e => e.Id);

        builder.HasOne(e => e.MediaWork)
            .WithMany(m => m.Duplicates)
            .HasForeignKey(e => e.MediaWorkId)
            .OnDelete(DeleteBehavior.Restrict);

        builder.HasOne(e => e.PrimaryMediaFile)
            .WithMany()
            .HasForeignKey(e => e.PrimaryMediaFileId)
            .OnDelete(DeleteBehavior.Restrict);

        builder.HasOne(e => e.DuplicateMediaFile)
            .WithMany()
            .HasForeignKey(e => e.DuplicateMediaFileId)
            .OnDelete(DeleteBehavior.Restrict);

        builder.HasIndex(e => new { e.MediaWorkId, e.MatchStatus })
            .HasDatabaseName("IX_MediaFileDuplicates_Work_Status")
            .IsUnique();

        builder.HasIndex(e => new { e.PrimaryMediaFileId, e.DuplicateMediaFileId })
            .HasDatabaseName("UQ_MediaFileDuplicates_Primary_Duplicate")
            .IsUnique();
    }
}