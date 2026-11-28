namespace MyVidLib.Domain.Media;

/// <summary>
/// Utilisateur de l'application.
/// </summary>
public class User
{
    public Guid Id { get; set; }

    public string NormalizedUserName { get; set; } = string.Empty;

    public UserRole Role { get; set; }

    public bool IsActive { get; set; }

    public DateTime CreatedAt { get; set; }

    public DateTime? ModifiedAt { get; set; }

    public virtual ICollection<WatchProgress> WatchProgresses { get; set; } = new List<WatchProgress>();
    public virtual ICollection<WatchSession> WatchSessions { get; set; } = new List<WatchSession>();
}