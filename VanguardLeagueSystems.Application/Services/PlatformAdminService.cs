using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using Microsoft.EntityFrameworkCore;
using VanguardLeagueSystems.Application.DTOs;
using VanguardLeagueSystems.Application.Interfaces;
using VanguardLeagueSystems.Domain.Entities;
using VanguardLeagueSystems.Domain.Enums;
using VanguardLeagueSystems.Infrastructure.Persistence;

namespace VanguardLeagueSystems.Application.Services;

public class PlatformAdminService : IPlatformAdminService
{
    private readonly VanguardDbContext _context;

    public PlatformAdminService(VanguardDbContext context)
    {
        _context = context;
    }

    public async Task<PlatformDashboardDto> GetPlatformMetricsAsync()
    {
        var dto = new PlatformDashboardDto();

        dto.TotalTenants = await _context.Tenants.IgnoreQueryFilters().CountAsync(t => !t.IsDeleted);
        dto.TotalActiveLeagues = await _context.Leagues.IgnoreQueryFilters().CountAsync(l => !l.IsDeleted && l.IsActive);
        dto.TotalPlayers = await _context.Players.IgnoreQueryFilters().CountAsync(p => !p.IsDeleted);

        dto.SystemTenants = await _context.Tenants
            .IgnoreQueryFilters()
            .Where(t => !t.IsDeleted)
            .Select(t => new TenantManagementDto
            {
                Id = t.Id,
                Name = t.Name,
                Identifier = t.Identifier,
                AdminEmail = t.AdminEmail,
                Status = t.Status.ToString(),
                Tier = t.Tier.ToString()
            })
            .ToListAsync();

        return dto;
    }

    public async Task UpdateTenantStatusAsync(Guid tenantId, string status)
    {
        var tenant = await _context.Tenants
            .IgnoreQueryFilters()
            .FirstOrDefaultAsync(t => t.Id == tenantId);

        if (tenant != null && Enum.TryParse<TenantStatus>(status, out var parsedStatus))
        {
            tenant.Status = parsedStatus;
            await _context.SaveChangesAsync();
        }
    }

    public async Task<List<UserManagementDto>> GetTenantUsersAsync(Guid tenantId)
    {
        return await _context.Set<ApplicationUser>()
            .IgnoreQueryFilters()
            .Where(u => !u.IsDeleted && u.TenantId == tenantId)
            .Select(u => new UserManagementDto
            {
                Id = u.Id,
                Email = u.Email ?? string.Empty,
                FirstName = u.FirstName,
                LastName = u.LastName,
                TenantId = u.TenantId,
                Roles = _context.UserRoles
                    .IgnoreQueryFilters()
                    .Where(ur => ur.UserId == u.Id)
                    .Join(_context.Roles.IgnoreQueryFilters(),
                        ur => ur.RoleId,
                        r => r.Id,
                        (ur, r) => r.Name ?? string.Empty)
                    .ToList()
            })
            .ToListAsync();
    }
}