namespace MyVidLib.Domain.Media;

/// <summary>
/// Rôle d'une association fichier/œuvre. Valeur unique: Primary.
/// </summary>
public enum AssignmentRole
{
    Primary,
}

/// <summary>
/// Statut de rapprochement pour une association fichier/œuvre ou un identifiant externe.
/// </summary>
public enum MatchStatus
{
    Proposed,
    Confirmed,
    Rejected,
    Manual,
}

/// <summary>
/// Type d'épisode.
/// </summary>
public enum EpisodeType
{
    Standard,
    Special,
    OAV,
    ONA,
    Other,
}

/// <summary>
/// Type de relation entre œuvres.
/// </summary>
public enum WorkRelationType
{
    Adaptation,
    Prequel,
    Sequel,
    SpinOff,
    DerivedFilm,
    Related,
    Parent,
    Child,
}