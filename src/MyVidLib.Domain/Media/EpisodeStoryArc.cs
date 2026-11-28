namespace MyVidLib.Domain.Media;

/// <summary>
/// Association entre un arc narratif et un épisode.
/// </summary>
public class EpisodeStoryArc
{
    public Guid Id { get; set; }

    public Guid StoryArcId { get; set; }

    public Guid EpisodeWorkId { get; set; }

    public DateTime CreatedAt { get; set; }

    public virtual StoryArc? StoryArc { get; set; }
    public virtual MediaWork? Episode { get; set; }
}