namespace VanguardLeagueSystems.Domain.Contracts;

/// <summary>
/// Defines that an entity supports soft deletion rather than hard deletion.
/// Used for data recovery within a SaaS environment.
/// </summary>
public interface ISoftDelete
{
    bool IsDeleted { get; set; }
    DateTimeOffset? DeletedAt { get; set; }
}