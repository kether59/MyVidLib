namespace MyVidLib.Domain.Media;

/// <summary>
/// Association entre une œuvre et une classification contrôlée (ex. Anime).
/// </summary>
public class MediaWorkClassification
{
    public Guid Id { get; set; }

    public Guid MediaWorkId { get; set; }

    public Guid ClassificationId { get; set; }

    public bool IsPrimary { get; set; }

    public DateTime CreatedAt { get; set; }

    public virtual MediaWork? MediaWork { get; set; }
    public virtual Classification? Classification { get; set; }
}