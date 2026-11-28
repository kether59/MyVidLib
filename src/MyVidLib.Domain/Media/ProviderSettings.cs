namespace MyVidLib.Domain.Media;

/// <summary>
/// Paramètre fournisseur non secret (activation, priorités, quotas).
/// Les secrets (clés API, jetons) ne doivent pas être stockés ici.
/// </summary>
public class ProviderSettings
{
    public Guid Id { get; set; }

    public Guid ProviderId { get; set; }

    public string Key { get; set; } = string.Empty;

    public string? Value { get; set; }

    public DateTime ModifiedAt { get; set; }

    public virtual Provider? Provider { get; set; }
}