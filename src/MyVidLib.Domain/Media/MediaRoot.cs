namespace MyVidLib.Domain.Media;

/// <summary>
/// Racine de stockage configurée : dossier local, lecteur externe ou partage réseau.
/// </summary>
public class MediaRoot
{
    public Guid Id { get; set; }

    /// <summary>Chemin physique absolu configuré.</summary>
    public string AbsolutePath { get; set; } = string.Empty;

    /// <summary>Statut de disponibilité actuel.</summary>
    public MediaRootAvailabilityStatus AvailabilityStatus { get; set; }

    /// <summary>Mode de sensibilité à la casse.</summary>
    public PathCaseMode PathCaseMode { get; set; }

    public DateTime CreatedAt { get; set; }

    public DateTime? ModifiedAt { get; set; }

    public virtual ICollection<MediaFile> Files { get; set; } = new List<MediaFile>();
    public virtual ICollection<MediaScan> Scans { get; set; } = new List<MediaScan>();
}