namespace MyVidLib.Domain.Media;

/// <summary>
/// Représente une exécution de scan avec son type, son état, ses horodatages, ses compteurs et son identifiant de corrélation.
/// </summary>
public class MediaScan
{
    public Guid Id { get; set; }

    public ScanType ScanType { get; set; }

    public ScanStatus Status { get; set; }

    public DateTime StartedAt { get; set; }

    public DateTime? CompletedAt { get; set; }

    public Guid? ScannedByUserId { get; set; }

    public Guid? MediaRootId { get; set; }

    public string? CorrelationId { get; set; }

    public virtual MediaRoot? MediaRoot { get; set; }
    public virtual ICollection<MediaScanScope> Scopes { get; set; } = new List<MediaScanScope>();
    public virtual ICollection<MediaScanError> Errors { get; set; } = new List<MediaScanError>();
}