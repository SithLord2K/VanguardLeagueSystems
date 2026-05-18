using System.ComponentModel.DataAnnotations;
using VanguardLeagueSystems.Domain.Contracts;
using VanguardLeagueSystems.Domain.Enums;

namespace VanguardLeagueSystems.Domain.Entities;

/// <summary>
/// Represents an organization or root league group using Vanguard League Systems.
/// This is the top-level entity for data isolation and subscription management.
/// </summary>
public class Tenant : ISoftDelete
{
    // Using Guids for security and scalability.
    public Guid Id { get; set; } = Guid.NewGuid();

    // The display name of the league organization (e.g., "City Youth Hockey")
    [Required]
    [MaxLength(200)]
    public string Name { get; set; } = string.Empty;

    // A unique identifier used in URLs (e.g., cityyouthhockey)
    // This allows for tenant resolution before login if desired.
    [Required]
    [MaxLength(100)]
    public string Identifier { get; set; } = string.Empty;

    // Contact information for the main administrator of this tenant
    [Required]
    [EmailAddress]
    public string AdminEmail { get; set; } = string.Empty;

    // Audit tracking
    public DateTimeOffset CreatedAt { get; set; } = DateTimeOffset.UtcNow;
    public DateTimeOffset? UpdatedAt { get; set; }

    // State and Billing Information
    public TenantStatus Status { get; set; } = TenantStatus.Active;
    public SubscriptionTier Tier { get; set; } = SubscriptionTier.Free;

    // Reference ID for your external payment provider (e.g., Stripe Customer ID).
    // The free .NET package for Stripe will populate this later.
    [MaxLength(100)]
    public string? ExternalPaymentId { get; set; }

    // Implements ISoftDelete
    public bool IsDeleted { get; set; }
    public DateTimeOffset? DeletedAt { get; set; }
}