using System;

namespace VanguardLeagueSystems.Application.DTOs;

public class TeamDto
{
    public Guid Id { get; set; }
    public Guid SeasonId { get; set; }
    public string Name { get; set; } = string.Empty;
    public string? PrimaryColorHex { get; set; }
    public string? SecondaryColorHex { get; set; }
}