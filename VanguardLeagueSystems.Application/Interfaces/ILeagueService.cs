using System;
using System.Collections.Generic;
using System.Threading.Tasks;
using VanguardLeagueSystems.Application.DTOs;

namespace VanguardLeagueSystems.Application.Interfaces;

/// <summary>
/// Defines the allowed business operations for Leagues.
/// </summary>
public interface ILeagueService
{
    Task<List<LeagueDto>> GetAllLeaguesAsync();
    Task<LeagueDto?> GetLeagueByIdAsync(Guid id);
    Task<Guid> CreateLeagueAsync(LeagueDto leagueDto);
    Task UpdateLeagueAsync(LeagueDto leagueDto);
    Task DeleteLeagueAsync(Guid id);
}