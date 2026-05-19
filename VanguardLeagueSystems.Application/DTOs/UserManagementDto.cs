using System;
using System.Collections.Generic;

namespace VanguardLeagueSystems.Application.DTOs;

public class UserManagementDto
{
    public Guid Id { get; set; }
    public string Email { get; set; } = string.Empty;
    public string FirstName { get; set; } = string.Empty;
    public string LastName { get; set; } = string.Empty;
    public Guid TenantId { get; set; }
    public List<string> Roles { get; set; } = new();
}