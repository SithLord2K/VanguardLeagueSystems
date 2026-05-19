using System;
using System.Collections.Generic;

namespace VanguardLeagueSystems.Application.DTOs;

public class PlatformDashboardDto
{
    public int TotalTenants { get; set; }
    public int TotalActiveLeagues { get; set; }
    public int TotalPlayers { get; set; }
    public List<TenantManagementDto> SystemTenants { get; set; } = new();
}