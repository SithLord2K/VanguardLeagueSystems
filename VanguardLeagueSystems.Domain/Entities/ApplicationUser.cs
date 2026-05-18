using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using Microsoft.AspNetCore.Identity;
using VanguardLeagueSystems.Domain.Contracts;

namespace VanguardLeagueSystems.Domain.Entities;

/// <summary>
/// Represents an authenticated login account within Vanguard League Systems.
/// Inherits from IdentityUser for ASP.NET Security, but implements our SaaS interfaces
/// directly since we cannot inherit from BaseEntity.
/// </summary>
public class ApplicationUser : IdentityUser<Guid>, IMustHaveTenant, ISoftDelete
{
    // Basic Profile Information
    [Required]
    [MaxLength(100)]
    public string FirstName { get; set; } = string.Empty;

    [Required]
    [MaxLength(100)]
    public string LastName { get; set; } = string.Empty;

    // NOTE: We removed 'public string Email' because IdentityUser<Guid> 
    // already provides Email, UserName, PasswordHash, etc.

    // --- SaaS Security Implementation ---

    // Implements IMustHaveTenant
    public Guid TenantId { get; set; }

    // Audit Tracking (Manually added since we don't have BaseEntity)
    public DateTimeOffset CreatedAt { get; set; } = DateTimeOffset.UtcNow;
    public DateTimeOffset? UpdatedAt { get; set; }

    // Implements ISoftDelete
    public bool IsDeleted { get; set; }
    public DateTimeOffset? DeletedAt { get; set; }

    // --- Domain Navigation ---

    // Optional: Link to a specific Player entity.
    public Guid? AssociatedPlayerId { get; set; }
    public virtual Player? AssociatedPlayer { get; set; }

    // The list of dynamic roles assigned to this specific user context.
    public virtual ICollection<ApplicationUserRole> UserRoles { get; set; } = new List<ApplicationUserRole>();
}