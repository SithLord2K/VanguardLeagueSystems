namespace VanguardLeagueSystems.Domain.Contracts;

/// <summary>
/// Defines that an entity MUST be isolated by a TenantId.
/// This is used by Infrastructure to automatically apply query filters.
/// </summary>
public interface IMustHaveTenant
{
    Guid TenantId { get; set; }
}