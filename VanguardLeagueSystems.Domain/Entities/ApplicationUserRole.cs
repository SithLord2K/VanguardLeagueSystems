using System.ComponentModel.DataAnnotations;
using VanguardLeagueSystems.Domain.Common;

namespace VanguardLeagueSystems.Domain.Entities;

/// <summary>
/// Securely assigns a dynamic, tenant-specific ApplicationRole to an ApplicationUser.
/// Must inherit BaseEntity to maintain Tenant isolation on the relationship record.
/// </summary>
public class ApplicationUserRole : BaseEntity
{
    // The User Id
    [Required]
    public Guid ApplicationUserId { get; set; }

    // The dynamic Role Id
    [Required]
    public Guid ApplicationRoleId { get; set; }

    // Audit trailing (UserRole assignment timestamp)
    public DateTimeOffset AssignedAt { get; set; } = DateTimeOffset.UtcNow;

    // Navigation Properties (Domain Documentation)
    public virtual ApplicationUser? ApplicationUser { get; set; }
    public virtual ApplicationRole? ApplicationRole { get; set; }
}