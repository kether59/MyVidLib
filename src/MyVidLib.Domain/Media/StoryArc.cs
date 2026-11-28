namespace MyVidLib.Domain.Media;

/// <summary>
/// Regroupement narratif facultatif d'épisodes appartenant à une série.
/// </summary>
public class StoryArc
{
    public Guid Id { get; set; }

    public Guid SeriesWorkId { get; set; }

    public int SortOrder { get; set; }

    public string Title { get; set; } = string.Empty;

    public DateTime CreatedAt { get; set; }

    public virtual MediaWork? Series { get; set; }
    public virtual ICollection<EpisodeStoryArc> EpisodeStoryArcs { get; set; } = new List<EpisodeStoryArc>();
}