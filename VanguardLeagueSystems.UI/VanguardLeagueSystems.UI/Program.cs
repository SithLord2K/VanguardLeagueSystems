using Microsoft.AspNetCore.Components.Authorization;
using Microsoft.AspNetCore.Identity;
using Microsoft.EntityFrameworkCore;
using MudBlazor.Services;
using VanguardLeagueSystems.Application.Interfaces;
using VanguardLeagueSystems.Application.Services;
using VanguardLeagueSystems.Domain.Entities;
using VanguardLeagueSystems.Infrastructure.Persistence;
using VanguardLeagueSystems.Infrastructure.Security;
using VanguardLeagueSystems.UI.Components;
using VanguardLeagueSystems.UI.Components.Account;

namespace VanguardLeagueSystems.UI
{
    public class Program
    {
        public static void Main(string[] args)
        {
            var builder = WebApplication.CreateBuilder(args);

            // Add services to the container.
            builder.Services.AddRazorComponents()
                .AddInteractiveServerComponents()
                .AddInteractiveWebAssemblyComponents()
                .AddAuthenticationStateSerialization();

            builder.Services.AddCascadingAuthenticationState();
            builder.Services.AddScoped<IdentityRedirectManager>();
            builder.Services.AddScoped<AuthenticationStateProvider, IdentityRevalidatingAuthenticationStateProvider>();

            // 1. Register the Tenant Provider (Scoped so it lives for the duration of the user's circuit)
            builder.Services.AddScoped<CurrentTenantProvider>();
            builder.Services.AddScoped<ImpersonationManager>();
            builder.Services.AddScoped<IdentityRevalidatingAuthenticationStateProvider>();

            // Intercept and decorate the primary AuthenticationStateProvider registration
            builder.Services.AddScoped<AuthenticationStateProvider>(sp =>
                new ImpersonatingAuthenticationStateProvider(
                    sp.GetRequiredService<IdentityRevalidatingAuthenticationStateProvider>(),
                    sp.GetRequiredService<ImpersonationManager>()));

            // 2. Register the Database Context
            builder.Services.AddDbContext<VanguardDbContext>(options =>
                options.UseSqlServer(builder.Configuration.GetConnectionString("DefaultConnection")));
            // Add MudBlazor Services
            builder.Services.AddMudServices();

            // --- Application Services ---
            builder.Services.AddScoped<ILeagueService, LeagueService>();
            builder.Services.AddScoped<ISeasonService, SeasonService>();
            builder.Services.AddScoped<ITeamService, TeamService>();
            builder.Services.AddScoped<IPlayerService, PlayerService>();
            builder.Services.AddScoped<IRosterService, RosterService>();
            builder.Services.AddScoped<ILeagueService, LeagueService>();

            builder.Services.AddIdentityCore<ApplicationUser>(options => options.SignIn.RequireConfirmedAccount = true)
                .AddRoles<ApplicationRole>() // Wire up your dynamic roles!
                .AddEntityFrameworkStores<VanguardDbContext>()
                .AddSignInManager()
                .AddDefaultTokenProviders();

            builder.Services.AddAuthentication(options =>
                {
                    options.DefaultScheme = IdentityConstants.ApplicationScheme;
                    options.DefaultSignInScheme = IdentityConstants.ExternalScheme;
                })
                .AddIdentityCookies();

            var connectionString = builder.Configuration.GetConnectionString("DefaultConnection") ?? throw new InvalidOperationException("Connection string 'DefaultConnection' not found.");
            builder.Services.AddDatabaseDeveloperPageExceptionFilter();

            var app = builder.Build();

            // Configure the HTTP request pipeline.
            if (app.Environment.IsDevelopment())
            {
                app.UseWebAssemblyDebugging();
                app.UseMigrationsEndPoint();
            }
            else
            {
                app.UseExceptionHandler("/Error");
                // The default HSTS value is 30 days. You may want to change this for production scenarios, see https://aka.ms/aspnetcore-hsts.
                app.UseHsts();
            }

            app.UseStatusCodePagesWithReExecute("/not-found", createScopeForStatusCodePages: true);
            app.UseHttpsRedirection();

            app.UseAntiforgery();

            app.MapStaticAssets();
            app.MapRazorComponents<App>()
                .AddInteractiveServerRenderMode()
                .AddInteractiveWebAssemblyRenderMode()
                .AddAdditionalAssemblies(typeof(Client._Imports).Assembly);

            // Add additional endpoints required by the Identity /Account Razor components.
            app.MapAdditionalIdentityEndpoints();

            app.Run();
        }
    }
}
