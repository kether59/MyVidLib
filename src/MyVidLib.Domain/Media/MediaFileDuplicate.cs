namespace MyVidLib.Domain.Media;

/// <summary>
/// Entité explicite de gestion des copies physiques supplémentaires d'une œuvre.
/// Chaque copie est un MediaFile distinct, avec sa propre racine, son chemin, ses caractéristiques techniques et ses empreintes.
/// La relation de doublon la rattache à une œuvre et au fichier principal de référence.
/// </summary>
public class MediaFileDuplicate
{
    public Guid Id { get; set; }

    public Guid MediaWorkId { get; set; }

    /// <summary>Fichier principal auquel la copie est comparée.</summary>
    public Guid PrimaryMediaFileId { get; set; }

    /// <summary>Fichier physique candidat/copieur.</summary>
    public Guid DuplicateMediaFileId { get; set; }

    public MatchStatus MatchStatus { get; set; }

    public DuplicateDetectionMethod DetectionMethod { get; set; }

    /// <summary>Confiance éventuelle (0-100).</summary>
    public int? Confidence { get; set; }

    public DateTime CreatedAt { get; set; }

    public DateTime? ConfirmedAt { get; set; }

    public Guid? ConfirmedByUserId { get; set; }

    public bool IsArchived { get; set; }

    public virtual MediaWork? MediaWork { get; set; }
    public virtual MediaFile? PrimaryMediaFile { get; set; }
    public virtual MediaFile? DuplicateMediaFile { get; set; }
}