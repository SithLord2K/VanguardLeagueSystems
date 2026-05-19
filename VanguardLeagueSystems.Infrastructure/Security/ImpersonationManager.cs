using System;
using System.Security.Claims;

namespace VanguardLeagueSystems.Infrastructure.Security;

public class ImpersonationManager
{
    private ClaimsPrincipal? _impersonatedUser;

    public event Action? OnImpersonationChanged;

    public bool IsImpersonating => _impersonatedUser != null;
    public ClaimsPrincipal? ImpersonatedUser => _impersonatedUser;

    public void StartImpersonation(ClaimsPrincipal targetUser)
    {
        _impersonatedUser = targetUser;
        OnImpersonationChanged?.Invoke();
    }

    public void StopImpersonation()
    {
        _impersonatedUser = null;
        OnImpersonationChanged?.Invoke();
    }
}