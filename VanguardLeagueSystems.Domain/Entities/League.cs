using System.ComponentModel.DataAnnotations;
using VanguardLeagueSystems.Domain.Common;
using VanguardLeagueSystems.Domain.Enums;
using VanguardLeagueSystems.Domain.ValueObjects;

namespace VanguardLeagueSystems.Domain.Entities;

/// <summary>
/// Represents a specific league structure (e.g., "Adult Rec Hockey", "Under-12 Soccer").
/// Defines the sport played and the core ruleset used by all seasons within this league.
/// </summary>
public class League : BaseEntity
{
    // The display name of the league.
    [Required]
    [MaxLength(200)]
    public string Name { get; set; } = string.Empty;

    // A brief summary or statement of purpose for the league.
    [MaxLength(500)]
    public string? Description { get; set; }

    // Identifies the sport. Important for the future Elo engine to know
    // how to treat game results.
    [Required]
    public SportType Sport { get; set; }

    /// <summary>
    /// Gets or sets the sport-specific rules configuration.
    /// This is an "Owned Entity" that Vanguard.Infrastructure will configure
    /// to store as a single JSON column in SQL Server.
    /// </summary>
    [Required]
    public SportConfiguration Configuration { get; set; } = new();

    // Administrative flag to quickly pause a league without deleting it.
    public bool IsActive { get; set; } = true;

    // Navigation properties (Domain Layer documentation)

    // The Tenant (organization) that owns this league.
    public virtual Tenant? Tenant { get; set; }

    // The historical and current list of seasons belonging to this league.
    public virtual ICollection<Season> Seasons { get; set; } = new List<Season>();
}