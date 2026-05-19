using System;
using System.Collections.Generic;
using System.Threading.Tasks;
using VanguardLeagueSystems.Application.DTOs;

namespace VanguardLeagueSystems.Application.Interfaces;

public interface IPlatformAdminService
{
    Task<PlatformDashboardDto> GetPlatformMetricsAsync();
    Task UpdateTenantStatusAsync(Guid tenantId, string status);
    Task<List<UserManagementDto>> GetTenantUsersAsync(Guid tenantId);
}