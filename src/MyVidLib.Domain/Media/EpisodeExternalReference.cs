namespace MyVidLib.Domain.Media;

/// <summary>
/// Référence externe de catalogue épisode (identifiant fournisseur + numéros saison/épisode fournisseur).
/// </summary>
public class EpisodeExternalReference
{
    public Guid Id { get; set; }

    public Guid ProviderId { get; set; }

    public EpisodeExternalKind ExternalKind { get; set; }

    public string ProviderValue { get; set; } = string.Empty;

    public int? ProviderSeasonNumber { get; set; }

    public int? ProviderEpisodeNumber { get; set; }

    public DateTime CreatedAt { get; set; }

    public virtual Provider? Provider { get; set; }
    public virtual ICollection<EpisodeExternalMapping> Mappings { get; set; } = new List<EpisodeExternalMapping>();
}