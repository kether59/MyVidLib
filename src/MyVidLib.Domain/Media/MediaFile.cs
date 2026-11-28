namespace MyVidLib.Domain.Media;

/// <summary>
/// Représentation d'un fichier physique unique dans une racine.
/// </summary>
public class MediaFile
{
    public Guid Id { get; set; }

    /// <summary>Identifiant de la racine de stockage.</summary>
    public Guid MediaRootId { get; set; }

    /// <summary>Identifiant de l'œuvre propriétaire du fichier (si assigné).</summary>
    public Guid? MediaWorkId { get; set; }

    /// <summary>Chemin relatif normalisé conservé pour l'affichage.</summary>
    public string RelativePath { get; set; } = string.Empty;

    /// <summary>Chemin relatif normalisé (clé canonique de comparaison et d'unicité).</summary>
    public string NormalizedRelativePath { get; set; } = string.Empty;

    /// <summary>Nom du fichier.</summary>
    public string Name { get; set; } = string.Empty;

    /// <summary>Extension du fichier.</summary>
    public string Extension { get; set; } = string.Empty;

    /// <summary>Taille du fichier en octets.</summary>
    public long SizeBytes { get; set; }

    /// <summary>Statut de disponibilité.</summary>
    public MediaFileAvailabilityStatus AvailabilityStatus { get; set; }

    /// <summary>Dernière observation (scan).</summary>
    public DateTime? LastSeenAt { get; set; }

    /// <summary>Dernière vérification fiable de présence ou absence.</summary>
    public DateTime? LastVerifiedAt { get; set; }

    /// <summary>Durée totale de l'épisode en ms, relative au début de l'épisode.</summary>
    public int? DurationMs { get; set; }

    public virtual MediaRoot? MediaRoot { get; set; }
}