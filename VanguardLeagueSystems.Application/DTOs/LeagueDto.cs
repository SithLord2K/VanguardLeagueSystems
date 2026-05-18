using System;
using VanguardLeagueSystems.Domain.Enums;
using VanguardLeagueSystems.Domain.ValueObjects;

namespace VanguardLeagueSystems.Application.DTOs;

/// <summary>
/// Lightweight, untracked representation of a League for UI consumption.
/// </summary>
public class LeagueDto
{
    public Guid Id { get; set; }
    public string Name { get; set; } = string.Empty;
    public string? Description { get; set; }
    public SportType Sport { get; set; }
    public bool IsActive { get; set; }

    // We pass the configuration directly so the UI knows how to render scoreboards
    public SportConfiguration Configuration { get; set; } = new();
}