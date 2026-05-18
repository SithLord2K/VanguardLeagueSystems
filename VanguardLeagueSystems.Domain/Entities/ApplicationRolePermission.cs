using System.ComponentModel.DataAnnotations;
using VanguardLeagueSystems.Domain.Common;

namespace VanguardLeagueSystems.Domain.Entities;

/// <summary>
/// Securely links a dynamic ApplicationRole to a granular permission constant.
/// Must inherit BaseEntity to maintain Tenant isolation on the relationship record.
/// </summary>
public class ApplicationRolePermission : BaseEntity
{
    // The dynamic Role Id
    [Required]
    public Guid ApplicationRoleId { get; set; }

    // The static permission constant (e.g., VanguardPermissions.Leagues.View)
    [Required]
    [MaxLength(200)]
    public string Permission { get; set; } = string.Empty;

    // Navigation Properties (Domain Documentation)
    public virtual ApplicationRole? ApplicationRole { get; set; }
}