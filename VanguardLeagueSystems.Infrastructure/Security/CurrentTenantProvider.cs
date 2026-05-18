using Microsoft.AspNetCore.Components.Authorization;
using System.Security.Claims;
using System.Threading.Tasks;
using System;
using System.Linq;

namespace VanguardLeagueSystems.Infrastructure.Security;

public class CurrentTenantProvider
{
    private readonly AuthenticationStateProvider _authenticationStateProvider;
    private Guid? _systemOverrideTenantId;

    // Synchronous property used by EF Core Query Filters
    public Guid TenantId { get; private set; }

    public CurrentTenantProvider(AuthenticationStateProvider authenticationStateProvider)
    {
        _authenticationStateProvider = authenticationStateProvider;
    }

    public void SetSystemOverride(Guid tenantId)
    {
        _systemOverrideTenantId = tenantId;
        TenantId = tenantId; // Update the sync property
    }

    public async Task<Guid> GetCurrentTenantIdAsync()
    {
        if (_systemOverrideTenantId.HasValue)
        {
            TenantId = _systemOverrideTenantId.Value;
            return TenantId;
        }

        var authState = await _authenticationStateProvider.GetAuthenticationStateAsync();
        var user = authState.User;

        if (user.Identity != null && user.Identity.IsAuthenticated)
        {
            var tenantClaim = user.Claims.FirstOrDefault(c => c.Type == "TenantId");
            if (tenantClaim != null && Guid.TryParse(tenantClaim.Value, out var tenantId))
            {
                TenantId = tenantId;
                return TenantId;
            }
        }

        TenantId = Guid.Empty;
        return TenantId;
    }
}