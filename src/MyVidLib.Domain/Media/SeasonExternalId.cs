namespace MyVidLib.Domain.Media;

/// <summary>
/// Identifiant externe d'une saison.
/// </summary>
public class SeasonExternalId
{
    public Guid Id { get; set; }

    public Guid SeasonId { get; set; }

    public Guid ProviderId { get; set; }

    public SeasonExternalKind ExternalKind { get; set; }

    public string ProviderValue { get; set; } = string.Empty;

    public MatchStatus Status { get; set; }

    public int? Confidence { get; set; }

    public int? ProviderSeasonNumber { get; set; }

    public DateTime CreatedAt { get; set; }

    public DateTime? ValidatedAt { get; set; }

    public Guid? ValidatedByUserId { get; set; }

    public virtual Season? Season { get; set; }
    public virtual Provider? Provider { get; set; }
}