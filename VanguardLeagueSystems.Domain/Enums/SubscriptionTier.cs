namespace VanguardLeagueSystems.Domain.Enums;

/// <summary>
/// Defines the available pricing tiers for Vanguard League Systems.
/// These tiers will later dictate feature access in the UI via Authorization Policies.
/// </summary>
public enum SubscriptionTier
{
    // Never remove or reorder existing enum values; only append to the end.
    Free = 0,
    Basic = 1,
    Pro = 2,
    Enterprise = 3
}