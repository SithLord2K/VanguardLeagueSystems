using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using Microsoft.AspNetCore.Identity;
using VanguardLeagueSystems.Domain.Contracts;

namespace VanguardLeagueSystems.Domain.Entities;

/// <summary>
/// Represents a dynamic, tenant-specific collection of permissions.
/// Inherits from IdentityRole for ASP.NET Security.
/// </summary>
public class ApplicationRole : IdentityRole<Guid>, IMustHaveTenant, ISoftDelete
{
    // NOTE: IdentityRole<Guid> already provides the 'Id' and 'Name' properties.

    [MaxLength(250)]
    public string? Description { get; set; }

    // --- SaaS Security Implementation ---

    // Implements IMustHaveTenant
    public Guid TenantId { get; set; }

    // Audit Tracking
    public DateTimeOffset CreatedAt { get; set; } = DateTimeOffset.UtcNow;
    public DateTimeOffset? UpdatedAt { get; set; }

    // Implements ISoftDelete
    public bool IsDeleted { get; set; }
    public DateTimeOffset? DeletedAt { get; set; }

    // --- Domain Navigation ---

    // The list of granular permissions assigned to this dynamic role.
    public virtual ICollection<ApplicationRolePermission> RolePermissions { get; set; } = new List<ApplicationRolePermission>();
}