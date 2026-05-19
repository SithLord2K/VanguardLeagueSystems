using System;
using System.Threading.Tasks;
using Microsoft.AspNetCore.Components.Authorization;

namespace VanguardLeagueSystems.Infrastructure.Security;

public class ImpersonatingAuthenticationStateProvider : AuthenticationStateProvider, IDisposable
{
    private readonly AuthenticationStateProvider _realStateProvider;
    private readonly ImpersonationManager _impersonationManager;

    public ImpersonatingAuthenticationStateProvider(
        AuthenticationStateProvider realStateProvider,
        ImpersonationManager impersonationManager)
    {
        _realStateProvider = realStateProvider;
        _impersonationManager = impersonationManager;

        _realStateProvider.AuthenticationStateChanged += OnRealAuthenticationStateChanged;
        _impersonationManager.OnImpersonationChanged += OnImpersonationManagerChanged;
    }

    private void OnRealAuthenticationStateChanged(Task<AuthenticationState> task)
    {
        NotifyAuthenticationStateChanged(GetAuthenticationStateAsync());
    }

    private void OnImpersonationManagerChanged()
    {
        NotifyAuthenticationStateChanged(GetAuthenticationStateAsync());
    }

    public override async Task<AuthenticationState> GetAuthenticationStateAsync()
    {
        var realState = await _realStateProvider.GetAuthenticationStateAsync();

        if (realState.User.IsInRole("PlatformAdmin") && _impersonationManager.IsImpersonating)
        {
            return new AuthenticationState(_impersonationManager.ImpersonatedUser!);
        }

        return realState;
    }

    public void Dispose()
    {
        _realStateProvider.AuthenticationStateChanged -= OnRealAuthenticationStateChanged;
        _impersonationManager.OnImpersonationChanged -= OnImpersonationManagerChanged;
    }
}