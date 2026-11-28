namespace MyVidLib.Domain.Media;

/// <summary>
/// Type d'identifiant externe fournisseur pour MediaWork (Movie/Series).
/// </summary>
public enum MediaWorkExternalKind
{
    Tmdb,
    Imdb,
    Animeka,
    AnimeSama,
    Other,
}

/// <summary>
/// Type d'identifiant externe fournisseur pour Season.
/// </summary>
public enum SeasonExternalKind
{
    SeasonNumber,
    Tmdb,
    Animeka,
    AnimeSama,
    Other,
}

/// <summary>
/// Type de référence externe d'épisode (catalogue fournisseur).
/// </summary>
public enum EpisodeExternalKind
{
    Tmdb,
    Animeka,
    AnimeSama,
    Imdb,
    Other,
}

/// <summary>
/// Statut de correspondance d'un mapping épisode externe.
/// </summary>
public enum EpisodeMapStatus
{
    Proposed,
    Confirmed,
    Rejected,
    Manual,
}