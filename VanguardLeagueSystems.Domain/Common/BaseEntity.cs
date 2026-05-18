using VanguardLeagueSystems.Domain.Contracts;

namespace VanguardLeagueSystems.Domain.Common;

/// <summary>
/// The foundational abstract class for Vanguard entities.
/// Combines unique identification, audit tracking, tenant isolation, and soft delete.
/// </summary>
public abstract class BaseEntity : IMustHaveTenant, ISoftDelete
{
    // We use Guid for SaaS IDs to prevent ID enumeration attacks
    // and make data merging easier if ever required.
    public Guid Id { get; set; } = Guid.NewGuid();

    // Implements IMustHaveTenant
    public Guid TenantId { get; set; }

    // Audit Tracking
    public DateTimeOffset CreatedAt { get; set; } = DateTimeOffset.UtcNow;
    public DateTimeOffset? UpdatedAt { get; set; }

    // Implements ISoftDelete
    public bool IsDeleted { get; set; }
    public DateTimeOffset? DeletedAt { get; set; }
}