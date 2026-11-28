namespace MyVidLib.Domain.Media;

/// <summary>
/// Regroupe les épisodes d'une série. Porte SeasonKind (Normal/Special/Unknown),
/// SeasonNumber (numéro métier nullable uniquement pour Unknown) et SortOrder (ordre
/// de tri interne obligatoire).
/// </summary>
public class Season
{
    public Guid Id { get; set; }

    public Guid SeriesWorkId { get; set; }

    public SeasonKind SeasonKind { get; set; }

    public int? SeasonNumber { get; set; }

    public int SortOrder { get; set; }

    public string Title { get; set; } = string.Empty;

    public DateTime CreatedAt { get; set; }

    public bool IsArchived { get; set; }

    public virtual MediaWork? Series { get; set; }
    public virtual ICollection<MediaWork> Episodes { get; set; } = new List<MediaWork>();
}