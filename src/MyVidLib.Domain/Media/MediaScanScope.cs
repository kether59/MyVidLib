namespace MyVidLib.Domain.Media;

/// <summary>
/// Trace de couverture de scan : pour chaque répertoire concerné, son chemin relatif normalisé
/// et le résultat de l'énumération (Completed, Inaccessible, Failed, NotReached).
/// </summary>
public class MediaScanScope
{
    public Guid Id { get; set; }

    public Guid MediaScanId { get; set; }

    public string RelativeDirectoryPath { get; set; } = string.Empty;

    public EnumerationStatus EnumerationStatus { get; set; }

    public DateTime StartedAt { get; set; }

    public DateTime? CompletedAt { get; set; }

    public virtual MediaScan? MediaScan { get; set; }
}