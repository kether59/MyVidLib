namespace MyVidLib.Domain.Media;

/// <summary>
/// Type de valeur pour les métadonnées.
/// </summary>
public enum MetadataValueType
{
    Text,
    Number,
    Date,
    List,
    Image,
    Structured,
}

/// <summary>
/// Provenance d'une valeur de métadonnée.
/// </summary>
public enum MetadataProvenance
{
    Provider,
    Manual,
    Local,
}

/// <summary>
/// État de validation d'une valeur candidate de métadonnée.
/// </summary>
public enum MetadataValidation
{
    Proposed,
    Validated,
    Rejected,
}