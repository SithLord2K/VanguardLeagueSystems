using System;

namespace VanguardLeagueSystems.Application.DTOs;

public class PlayerDto
{
    public Guid Id { get; set; }
    public string FirstName { get; set; } = string.Empty;
    public string LastName { get; set; } = string.Empty;
    public string? ContactEmail { get; set; }
    public DateTime DateOfBirth { get; set; }

    // UI Helper property
    public string FullName => $"{FirstName} {LastName}";
}