namespace MyVidLib.Domain.Media;

/// <summary>
/// Taxonomie contrôlée et extensible (code stable, libellé et éventuellement parent)
/// reliée à MediaWork par MediaWorkClassification.
/// </summary>
public class Classification
{
    public Guid Id { get; set; }

    public string Code { get; set; } = string.Empty;

    public string Label { get; set; } = string.Empty;

    public Guid? ParentClassificationId { get; set; }

    public bool IsDeprecated { get; set; }

    public virtual Classification? ParentClassification { get; set; }
    public virtual ICollection<MediaWorkClassification> MediaWorkClassifications { get; set; } = new List<MediaWorkClassification>();
}