using System;
using System.Linq;
using System.Threading;
using System.Threading.Tasks;
using Microsoft.AspNetCore.Identity;
using Microsoft.AspNetCore.Identity.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.ChangeTracking;
using VanguardLeagueSystems.Domain.Common;
using VanguardLeagueSystems.Domain.Contracts;
using VanguardLeagueSystems.Domain.Entities;
using VanguardLeagueSystems.Infrastructure.Security;

namespace VanguardLeagueSystems.Infrastructure.Persistence;

/// <summary>
/// The main database context for Vanguard League Systems.
/// Configured for multi-tenancy, soft deletes, and automatic audit tracking.
/// </summary>
public class VanguardDbContext : IdentityDbContext<IdentityUser<Guid>, IdentityRole<Guid>, Guid>
{
    private readonly CurrentTenantProvider _tenantProvider;

    public VanguardDbContext(
        DbContextOptions<VanguardDbContext> options,
        CurrentTenantProvider tenantProvider)
        : base(options)
    {
        _tenantProvider = tenantProvider;
    }

    // --- Data Sets (SQL Tables) ---
    public DbSet<Tenant> Tenants { get; set; }
    public DbSet<League> Leagues { get; set; }
    public DbSet<Season> Seasons { get; set; }
    public DbSet<Team> Teams { get; set; }
    public DbSet<Player> Players { get; set; }
    public DbSet<RosterSlot> RosterSlots { get; set; }
    public DbSet<ApplicationUser> ApplicationUsers { get; set; }
    public DbSet<ApplicationRole> ApplicationRoles { get; set; }
    public DbSet<ApplicationRolePermission> ApplicationRolePermissions { get; set; }
    public DbSet<ApplicationUserRole> ApplicationUserRoles { get; set; }

    protected override void OnModelCreating(ModelBuilder modelBuilder)
    {
        base.OnModelCreating(modelBuilder);

        // 1. JSON Column Mapping
        // This tells SQL Server to store the complex SportConfiguration object 
        // as a native JSON string within the League table.
        modelBuilder.Entity<League>().OwnsOne(l => l.Configuration, config =>
        {
            config.ToJson();
        });

        // 2. Unique Constraints for Join Tables
        // Since these inherit from BaseEntity, they have an 'Id' primary key, 
        // but we must prevent duplicate assignments.
        modelBuilder.Entity<RosterSlot>()
            .HasIndex(rs => new { rs.TeamId, rs.PlayerId })
            .IsUnique();

        modelBuilder.Entity<ApplicationRolePermission>()
            .HasIndex(arp => new { arp.ApplicationRoleId, arp.Permission })
            .IsUnique();

        modelBuilder.Entity<ApplicationUserRole>()
            .HasIndex(aur => new { aur.ApplicationUserId, aur.ApplicationRoleId })
            .IsUnique();

        // 3. Restrict Cascade Deletes
        // In a SaaS, you rarely want cascading deletes. We rely on our ISoftDelete 
        // interceptor instead. This prevents SQL Server constraint errors.
        foreach (var relationship in modelBuilder.Model.GetEntityTypes()
            .SelectMany(e => e.GetForeignKeys()))
        {
            relationship.DeleteBehavior = DeleteBehavior.Restrict;
        }

        // --- GLOBAL QUERY FILTERS ---
        // These run automatically on EVERY LINQ query.
        // They ensure users never see soft-deleted data or data belonging to other tenants.

        modelBuilder.Entity<ApplicationUser>().HasQueryFilter(e => !e.IsDeleted && e.TenantId == _tenantProvider.TenantId);
        modelBuilder.Entity<ApplicationRole>().HasQueryFilter(e => !e.IsDeleted && e.TenantId == _tenantProvider.TenantId);
        modelBuilder.Entity<League>().HasQueryFilter(e => !e.IsDeleted && e.TenantId == _tenantProvider.TenantId);
        modelBuilder.Entity<Season>().HasQueryFilter(e => !e.IsDeleted && e.TenantId == _tenantProvider.TenantId);
        modelBuilder.Entity<Team>().HasQueryFilter(e => !e.IsDeleted && e.TenantId == _tenantProvider.TenantId);
        modelBuilder.Entity<Player>().HasQueryFilter(e => !e.IsDeleted && e.TenantId == _tenantProvider.TenantId);
        modelBuilder.Entity<RosterSlot>().HasQueryFilter(e => !e.IsDeleted && e.TenantId == _tenantProvider.TenantId);

        // Note: Tenant entity itself is not filtered by TenantId (it is the root), but it IS soft-delete filtered.
        modelBuilder.Entity<Tenant>().HasQueryFilter(e => !e.IsDeleted);
    }
   

    /// <summary>
    /// Intercepts save operations to automatically handle Audit Timestamps, 
    /// Soft Deletions, and TenantId enforcement based on entity interfaces.
    /// </summary>
    public override async Task<int> SaveChangesAsync(CancellationToken cancellationToken = default)
    {
        Guid currentTenantId = await _tenantProvider.GetCurrentTenantIdAsync();

        foreach (var entry in ChangeTracker.Entries())
        {
            if (entry.Entity is ISoftDelete softDeleteEntity && entry.State == EntityState.Deleted)
            {
                entry.State = EntityState.Modified;
                softDeleteEntity.IsDeleted = true;
                softDeleteEntity.DeletedAt = DateTimeOffset.UtcNow;
            }

            if (entry.Entity is BaseEntity baseEntity)
            {
                switch (entry.State)
                {
                    case EntityState.Added:
                        baseEntity.CreatedAt = DateTimeOffset.UtcNow;

                        if (currentTenantId == Guid.Empty)
                        {
                            throw new InvalidOperationException("Cannot insert tenant-isolated data without a valid tenant context.");
                        }

                        baseEntity.TenantId = currentTenantId;
                        break;

                    case EntityState.Modified:
                        baseEntity.UpdatedAt = DateTimeOffset.UtcNow;
                        entry.Property(nameof(BaseEntity.TenantId)).IsModified = false;
                        entry.Property(nameof(BaseEntity.CreatedAt)).IsModified = false;
                        break;
                }
            }
        }

        return await base.SaveChangesAsync(cancellationToken);
    }
}