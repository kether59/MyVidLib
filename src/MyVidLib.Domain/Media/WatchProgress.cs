namespace MyVidLib.Domain.Media;

/// <summary>
/// Progression courante unique par utilisateur et œuvre.
/// </summary>
public class WatchProgress
{
    public Guid Id { get; set; }

    public Guid UserId { get; set; }

    public Guid MediaWorkId { get; set; }

    /// <summary>Position relative à l'œuvre lue (millisecondes).</summary>
    public long? PositionMs { get; set; }

    /// <summary>Durée de référence (millisecondes).</summary>
    public long? DurationMs { get; set; }

    /// <summary>Indique si la fin du média a été atteinte pour la session (indépendant de Watched).</summary>
    public bool? MediaEndReached { get; set; }

    public WatchState Watched { get; set; }

    public Guid? LastMediaFileId { get; set; }

    public Guid? LastMediaFileAssignmentId { get; set; }

    public DateTime UpdatedAt { get; set; }

    public DateTime? LastWatchedAt { get; set; }

    public WatchProgressSource Source { get; set; }

    public virtual User? User { get; set; }
    public virtual MediaWork? MediaWork { get; set; }
}