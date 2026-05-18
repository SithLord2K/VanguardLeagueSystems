using System;

namespace VanguardLeagueSystems.Application.DTOs;

public class RosterSlotDto
{
    public Guid Id { get; set; }
    public Guid TeamId { get; set; }
    public Guid PlayerId { get; set; }

    // Denormalized for the UI so we don't have to fetch the whole Player object
    public string PlayerName { get; set; } = string.Empty;

    public string? JerseyNumber { get; set; }
    public string? Position { get; set; }
    public bool IsCaptain { get; set; }
}