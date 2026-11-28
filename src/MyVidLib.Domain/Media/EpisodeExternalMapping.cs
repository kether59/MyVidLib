namespace MyVidLib.Domain.Media;

/// <summary>
/// Mapping externe d'épisode vers une œuvre de type Episode.
/// </summary>
public class EpisodeExternalMapping
{
    public Guid Id { get; set; }

    public Guid EpisodeExternalReferenceId { get; set; }

    public Guid EpisodeWorkId { get; set; }

    public EpisodeMapStatus Status { get; set; }

    public int? Confidence { get; set; }

    public DateTime CreatedAt { get; set; }

    public DateTime? ValidatedAt { get; set; }

    public Guid? ValidatedByUserId { get; set; }

    public virtual EpisodeExternalReference? EpisodeExternalReference { get; set; }
    public virtual MediaWork? EpisodeWork { get; set; }
}