using System.ComponentModel.DataAnnotations;
using VanguardLeagueSystems.Domain.Common;

namespace VanguardLeagueSystems.Domain.Entities;

/// <summary>
/// Represents a distinct time period of play for a specific League (e.g., "Fall 2024").
/// All teams, rosters, schedules, and standings contextually belong to a Season.
/// </summary>
public class Season : BaseEntity
{
    // The ID of the parent league.
    [Required]
    public Guid LeagueId { get; set; }

    // The display name of the season (e.g., "2024 Winter Classic").
    [Required]
    [MaxLength(200)]
    public string Name { get; set; } = string.Empty;

    // When competition officially begins.
    [Required]
    public DateTimeOffset StartDate { get; set; }

    // When competition is expected to end.
    // Can be null if the season is just being configured.
    public DateTimeOffset? EndDate { get; set; }

    // Domain flag: Is this season actively playing games right now?
    public bool IsActive { get; set; }

    // Domain flag: Is the league accepting team registrations for this season?
    public bool IsRegistrationOpen { get; set; }

    // Domain flag: Have the finals completed and the season archived?
    public bool IsCompleted { get; set; }

    // Navigation properties (Domain Layer documentation)

    // The parent league this season belongs to.
    public virtual League? League { get; set; }

    // We will add collections of Teams, Schedules, etc., here later.
}