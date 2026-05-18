using System;
using System.Collections.Generic;
using System.Threading.Tasks;
using VanguardLeagueSystems.Application.DTOs;

namespace VanguardLeagueSystems.Application.Interfaces;

public interface ISeasonService
{
    Task<List<SeasonDto>> GetSeasonsByLeagueIdAsync(Guid leagueId);
    Task<SeasonDto?> GetSeasonByIdAsync(Guid id);
    Task<Guid> CreateSeasonAsync(SeasonDto seasonDto);
    Task UpdateSeasonAsync(SeasonDto seasonDto);
    Task DeleteSeasonAsync(Guid id);
}