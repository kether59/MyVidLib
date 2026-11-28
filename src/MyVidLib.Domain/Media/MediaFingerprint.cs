namespace MyVidLib.Domain.Media;

/// <summary>
/// Empreinte calculée d'un MediaFile, distincte de sa taille.
/// </summary>
public class MediaFingerprint
{
    public Guid MediaFileId { get; set; }

    public FingerprintAlgorithm Algorithm { get; set; }

    public FingerprintKind FingerprintKind { get; set; }

    /// <summary>Valeur de hash encodée en hex/base64.</summary>
    public string HashValue { get; set; } = string.Empty;

    public DateTime? CalculatedAt { get; set; }

    public FingerprintCalculationStatus CalculationStatus { get; set; }

    public virtual MediaFile? MediaFile { get; set; }
}