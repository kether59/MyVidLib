namespace MyVidLib.Domain.Media;

/// <summary>
/// Rôle utilisateur.
/// </summary>
public enum UserRole
{
    Administrator,
    User,
}

/// <summary>
/// État de progression de visionnage.
/// </summary>
public enum WatchState
{
    Unwatched,
    InProgress,
    Watched,
}

/// <summary>
/// Source de mise à jour de la progression.
/// </summary>
public enum WatchProgressSource
{
    Manual,
    Player,
}

/// <summary>
/// État d'une session de visionnage.
/// </summary>
public enum WatchSessionStatus
{
    InProgress,
    EndedNormally,
    Interrupted,
    Error,
}