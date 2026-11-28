namespace MyVidLib.Domain.Media;

/// <summary>
/// Enregistrement d'une session de lecture (une ligne par session).
/// </summary>
public class WatchSession
{
    public Guid Id { get; set; }

    public Guid UserId { get; set; }

    public Guid MediaWorkId { get; set; }

    public DateTime StartedAt { get; set; }

    public DateTime? EndedAt { get; set; }

    public WatchSessionStatus Status { get; set; }

    public long? PositionMs { get; set; }

    public long? DurationMs { get; set; }

    /// <summary>Fin naturelle du média pour cette session (indépendant de Watched).</summary>
    public bool? MediaEndReached { get; set; }

    /// <summary>Source de la session.</summary>
    public WatchProgressSource Source { get; set; }

    /// <summary>Dernière persistance de checkpoint.</summary>
    public DateTime? LastCheckpointAt { get; set; }

    public Guid? MediaFileId { get; set; }

    public Guid? MediaFileAssignmentId { get; set; }

    public virtual User? User { get; set; }
    public virtual MediaWork? MediaWork { get; set; }
    public virtual MediaFile? MediaFile { get; set; }
    public virtual MediaFileAssignment? MediaFileAssignment { get; set; }
}