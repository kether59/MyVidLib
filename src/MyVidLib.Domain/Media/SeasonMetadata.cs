namespace MyVidLib.Domain.Media;

/// <summary>
/// Valeur candidate de métadonnée pour une saison.
/// </summary>
public class SeasonMetadata
{
    public Guid Id { get; set; }

    public Guid SeasonId { get; set; }

    public string FieldName { get; set; } = string.Empty;

    public string LocaleCode { get; set; } = "und";

    public MetadataValueType ValueType { get; set; }

    /// <summary>Valeur brute la plus récemment observée (source fournisseur).</summary>
    public string? RawValue { get; set; }

    /// <summary>Valeur candidate canonique encodée en JSON selon ValueType.</summary>
    public string CandidateValue { get; set; } = string.Empty;

    public MetadataProvenance Provenance { get; set; }

    public Guid? ProviderId { get; set; }

    public MetadataValidation Validation { get; set; }

    public bool IsSelected { get; set; }

    public DateTime? SelectedAt { get; set; }

    public DateTime? FetchedAt { get; set; }

    public DateTime? LastFetchedAt { get; set; }

    public DateTime? ExpiresAt { get; set; }

    public DateTime CreatedAt { get; set; }

    public virtual Season? Season { get; set; }
    public virtual Provider? Provider { get; set; }
}