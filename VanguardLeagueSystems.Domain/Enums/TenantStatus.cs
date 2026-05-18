namespace VanguardLeagueSystems.Domain.Enums;

/// <summary>
/// Defines the operational status of a tenant account.
/// </summary>
public enum TenantStatus
{
    // The account is in good standing
    Active = 0,

    // The system administrator has locked the account (e.g., TOS violation)
    Deactivated = 1,

    // Automated payment failed; account is in grace period or restricted
    PastDue = 2,

    // The user has requested cancellation, but the paid period isn't over yet
    Canceling = 3
}