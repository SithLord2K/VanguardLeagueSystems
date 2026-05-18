using System.ComponentModel.DataAnnotations;
using VanguardLeagueSystems.Domain.Common;

namespace VanguardLeagueSystems.Domain.Entities;

/// <summary>
/// Represents an individual competitor registered within a Tenant organization.
/// Players exist at the Tenant level and can be assigned to rosters across different seasons.
/// </summary>
public class Player : BaseEntity
{
    // Basic Information
    [Required]
    [MaxLength(100)]
    public string FirstName { get; set; } = string.Empty;

    [Required]
    [MaxLength(100)]
    public string LastName { get; set; } = string.Empty;

    // Optional email for player notifications, separate from potential login accounts.
    [MaxLength(256)]
    [EmailAddress]
    public string? ContactEmail { get; set; }

    // Vital for sport age verification logic.
    [Required]
    public DateTime DateOfBirth { get; set; }

    // Security Link (Placeholder for now)
    // In the future, this might link to an ApplicationUser ID if the player
    // is granted login access to view their own stats.
    public string? ApplicationUserId { get; set; }

    // Navigation Properties (Domain Documentation)

    // The list of teams this player has played for.
    public virtual ICollection<RosterSlot> RosterAssignments { get; set; } = new List<RosterSlot>();
}