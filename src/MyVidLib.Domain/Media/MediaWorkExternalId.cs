namespace MyVidLib.Domain.Media;

/// <summary>
/// Identifiant externe d'une œuvre de type Movie ou Series.
/// </summary>
public class MediaWorkExternalId
{
    public Guid Id { get; set; }

    public Guid MediaWorkId { get; set; }

    public Guid ProviderId { get; set; }

    public MediaWorkExternalKind ExternalKind { get; set; }

    public string ProviderValue { get; set; } = string.Empty;

    public MatchStatus Status { get; set; }

    public int? Confidence { get; set; }

    public DateTime CreatedAt { get; set; }

    public DateTime? ValidatedAt { get; set; }

    public Guid? ValidatedByUserId { get; set; }

    public DateTime? ArchivedAt { get; set; }

    public virtual MediaWork? MediaWork { get; set; }
    public virtual Provider? Provider { get; set; }
}