namespace MyVidLib.Domain.Media;

/// <summary>
/// Relation narrative ou éditoriale entre deux œuvres.
/// </summary>
public class MediaWorkRelation
{
    public Guid Id { get; set; }

    public Guid SourceWorkId { get; set; }

    public Guid TargetWorkId { get; set; }

    public WorkRelationType RelationType { get; set; }

    public DateTime CreatedAt { get; set; }

    public virtual MediaWork? SourceWork { get; set; }
    public virtual MediaWork? TargetWork { get; set; }
}