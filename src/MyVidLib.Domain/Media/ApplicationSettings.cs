namespace MyVidLib.Domain.Media;

/// <summary>
/// Paramètre global applicatif (clé/valeur non secrète).
/// Les secrets ne doivent jamais être stockés en clair dans cette table.
/// </summary>
public class ApplicationSettings
{
    public string Key { get; set; } = string.Empty;

    public string? Value { get; set; }

    public DateTime ModifiedAt { get; set; }
}