using System.ComponentModel.DataAnnotations;
using VanguardLeagueSystems.Domain.Common;

namespace VanguardLeagueSystems.Domain.Entities;

/// <summary>
/// Represents a group of players competing in a specific Season iteration.
/// </summary>
public class Team : BaseEntity
{
    // The parent Season this team belongs to.
    [Required]
    public Guid SeasonId { get; set; }

    [Required]
    [MaxLength(200)]
    public string Name { get; set; } = string.Empty;

    // Aesthetic data for scheduling UIs.
    [MaxLength(50)]
    public string? PrimaryColorHex { get; set; }
    [MaxLength(50)]
    public string? SecondaryColorHex { get; set; }

    // Navigation Properties (Domain Documentation)

    // The parent Season.
    public virtual Season? Season { get; set; }

    // The roster of players assigned to this specific team iteration.
    public virtual ICollection<RosterSlot> Roster { get; set; } = new List<RosterSlot>();
}