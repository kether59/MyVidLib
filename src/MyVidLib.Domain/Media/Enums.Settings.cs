namespace MyVidLib.Domain.Media;

/// <summary>
/// Clés de paramètres globaux d'application.
/// </summary>
public enum ApplicationSettingsKey
{
    DatabasePath,
    MetadataRefreshIntervalDays,
    AutoWatchedThresholdPercent,
    SessionReconciliationDelayMinutes,
}

/// <summary>
/// Clés de paramètres utilisateur individuels.
/// </summary>
public enum UserSettingsKey
{
    PreferredLocale,
    PersonalAutoWatchedThresholdPercent,
    PreferredPlaybackQuality,
}

/// <summary>
/// Clés de paramètres fournisseur.
/// </summary>
public enum ProviderSettingsKey
{
    IsEnabled,
    Priority,
    DailyQuota,
    RequestTimeoutSeconds,
}