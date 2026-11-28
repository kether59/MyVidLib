namespace MyVidLib.Domain.Media;

/// <summary>
/// Fournisseur externe de métadonnées (TMDB, Animeka, Anime-Sama, IMDb).
/// </summary>
public class Provider
{
    public Guid Id { get; set; }

    public string Code { get; set; } = string.Empty;

    public string Name { get; set; } = string.Empty;

    public bool IsEnabled { get; set; }

    public int Priority { get; set; }

    public int? DailyQuota { get; set; }

    public int? RequestTimeoutSeconds { get; set; }

    public DateTime? LastAccessedAt { get; set; }

    public virtual ICollection<MediaWorkExternalId> MediaWorkExternalIds { get; set; } = new List<MediaWorkExternalId>();
    public virtual ICollection<SeasonExternalId> SeasonExternalIds { get; set; } = new List<SeasonExternalId>();
    public virtual ICollection<EpisodeExternalReference> EpisodeExternalReferences { get; set; } = new List<EpisodeExternalReference>();
    public virtual ICollection<MediaWorkMetadata> MediaWorkMetadata { get; set; } = new List<MediaWorkMetadata>();
    public virtual ICollection<SeasonMetadata> SeasonMetadata { get; set; } = new List<SeasonMetadata>();
}