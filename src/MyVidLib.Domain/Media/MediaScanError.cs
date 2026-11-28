namespace MyVidLib.Domain.Media;

/// <summary>
/// Erreur survenue pendant un scan (par fichier ou racine).
/// </summary>
public class MediaScanError
{
    public Guid Id { get; set; }

    public Guid MediaScanId { get; set; }

    public ScanErrorSeverity Severity { get; set; }

    /// <summary>Code technique de l'erreur.</summary>
    public string ErrorCode { get; set; } = string.Empty;

    /// <summary>Message sûr (sans données sensibles).</summary>
    public string SafeMessage { get; set; } = string.Empty;

    /// <summary>Phase du scan où l'erreur est survenue.</summary>
    public string Phase { get; set; } = string.Empty;

    /// <summary>Chemin relatif éventuel.</summary>
    public string? RelativePath { get; set; }

    public DateTime OccurredAt { get; set; }

    public virtual MediaScan? MediaScan { get; set; }
}