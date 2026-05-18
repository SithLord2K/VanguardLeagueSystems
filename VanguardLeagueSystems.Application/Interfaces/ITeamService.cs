using System;
using System.Collections.Generic;
using System.Threading.Tasks;
using VanguardLeagueSystems.Application.DTOs;

namespace VanguardLeagueSystems.Application.Interfaces;

public interface ITeamService
{
    Task<List<TeamDto>> GetTeamsBySeasonIdAsync(Guid seasonId);
    Task<TeamDto?> GetTeamByIdAsync(Guid id);
    Task<Guid> CreateTeamAsync(TeamDto teamDto);
    Task UpdateTeamAsync(TeamDto teamDto);
    Task DeleteTeamAsync(Guid id);
}