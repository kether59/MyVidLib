namespace MyVidLib.Domain.Media;

/// <summary>
/// Préférence individuelle utilisateur (clé/valeur non secrète).
/// </summary>
public class UserSettings
{
    public Guid Id { get; set; }

    public Guid UserId { get; set; }

    public string Key { get; set; } = string.Empty;

    public string? Value { get; set; }

    public DateTime ModifiedAt { get; set; }

    public virtual User? User { get; set; }
}