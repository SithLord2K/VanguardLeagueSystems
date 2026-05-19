using System;

namespace VanguardLeagueSystems.Application.DTOs;

public class TenantManagementDto
{
    public Guid Id { get; set; }
    public string Name { get; set; } = string.Empty;
    public string Identifier { get; set; } = string.Empty;
    public string AdminEmail { get; set; } = string.Empty;
    public string Status { get; set; } = string.Empty;
    public string Tier { get; set; } = string.Empty;
}