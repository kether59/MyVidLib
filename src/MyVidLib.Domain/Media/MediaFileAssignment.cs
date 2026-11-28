namespace MyVidLib.Domain.Media;

/// <summary>
/// Lien entre une œuvre lisible (Movie ou Episode) et son fichier principal actuellement retenu.
/// Une Series ne peut jamais recevoir de fichier vidéo directement associé.
/// </summary>
public class MediaFileAssignment
{
    public Guid Id { get; set; }

    public Guid MediaWorkId { get; set; }

    public Guid MediaFileId { get; set; }

    public AssignmentRole Role { get; set; }

    public MatchStatus MatchStatus { get; set; }

    public DateTime AssociatedAt { get; set; }

    public Guid? ConfirmedByUserId { get; set; }

    public DateTime? ConfirmedAt { get; set; }

    public bool IsArchived { get; set; }

    public virtual MediaWork? MediaWork { get; set; }
    public virtual MediaFile? MediaFile { get; set; }
}