using System.ComponentModel.DataAnnotations;
using VanguardLeagueSystems.Domain.Common;

namespace VanguardLeagueSystems.Domain.Entities;

/// <summary>
/// A join entity representing a Player's assignment to a specific Team's roster.
/// Must inherit BaseEntity to maintain Tenant isolation on the relationship record.
/// </summary>
public class RosterSlot : BaseEntity
{
    [Required]
    public Guid TeamId { get; set; }

    [Required]
    public Guid PlayerId { get; set; }

    // Roster-specific context data.
    [MaxLength(10)]
    public string? JerseyNumber { get; set; }

    // E.g., Goalie, Forward, Defender. Can be specific per sport.
    [MaxLength(100)]
    public string? Position { get; set; }

    // Flag for identifying the team captain.
    public bool IsCaptain { get; set; }

    // Navigation Properties (Domain Documentation)
    public virtual Team? Team { get; set; }
    public virtual Player? Player { get; set; }
}