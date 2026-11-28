
namespace MyVidLib.Domain.Media;

/// <summary>
/// Algorithme d'empreinte.
/// </summary>
public enum FingerprintAlgorithm
{
    Sha256,
    Blake3,
    Md5,
}

/// <summary>
/// Type d'empreinte (partielle ou complète).
/// </summary>
public enum FingerprintKind
{
    Partial,
    Full,
}

/// <summary>
/// Statut de calcul d'empreinte.
/// </summary>
public enum FingerprintCalculationStatus
{
    Pending,
    Succeeded,
    Failed,
}

/// <summary>
/// Méthode de détection de doublon.
/// </summary>
public enum DuplicateDetectionMethod
{
    Metadata,
    SizeAndDate,
    PartialFingerprint,
    FullFingerprint,
    Manual,
}

/// <summary>
/// Type de scan.
/// </summary>
public enum ScanType
{
    Full,
    Incremental,
    Manual,
}

/// <summary>
/// État d'un scan.
/// </summary>
public enum ScanStatus
{
    Completed,
    Partial,
    Cancelled,
    Failed,
}

/// <summary>
/// État d'énumération d'une portée de scan.
/// </summary>
public enum EnumerationStatus
{
    Completed,
    Inaccessible,
    Failed,
    NotReached,
}

/// <summary>
/// Sévérité d'une erreur de scan.
/// </summary>
public enum ScanErrorSeverity
{
    Information,
    Warning,
    Error,
    Critical,
}
