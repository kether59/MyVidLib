namespace MyVidLib.Domain.Media;

/// <summary>
/// Discriminateur de type d'œuvre pour l'héritage TPH.
/// </summary>
public enum WorkType
{
    /// <summary>Film.</summary>
    Movie,

    /// <summary>Série.</summary>
    Series,

    /// <summary>Épisode.</summary>
    Episode,
}

/// <summary>
/// Statut de catalogue d'une œuvre.
/// </summary>
public enum CatalogueStatus
{
    Accessible,
    Inaccessible,
    Disabled,
    Unknown,
}

/// <summary>
/// Type de saison.
/// </summary>
public enum SeasonKind
{
    /// <summary>Saison ordinaire (numéro positif).</summary>
    Normal,

    /// <summary>Saison spéciale/OAV (numéro 0).</summary>
    Special,

    /// <summary>Saison technique inconnue (SeasonNumber = null).</summary>
    Unknown,
}

/// <summary>
/// Statut de disponibilité d'une racine média.
/// </summary>
public enum MediaRootAvailabilityStatus
{
    Accessible,
    Inaccessible,
    Disabled,
    Unknown,
}

/// <summary>
/// Statut de disponibilité d'un fichier média.
/// </summary>
public enum MediaFileAvailabilityStatus
{
    Present,
    Missing,
    Unknown,
}

/// <summary>
/// Mode de sensibilité à la casse pour la comparaison de chemins.
/// </summary>
public enum PathCaseMode
{
    Insensitive,
    Sensitive,
}