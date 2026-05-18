using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using Microsoft.EntityFrameworkCore;
using VanguardLeagueSystems.Application.DTOs;
using VanguardLeagueSystems.Application.Interfaces;
using VanguardLeagueSystems.Domain.Entities;
using VanguardLeagueSystems.Infrastructure.Persistence;

namespace VanguardLeagueSystems.Application.Services;

public class SeasonService : ISeasonService
{
    private readonly VanguardDbContext _context;

    public SeasonService(VanguardDbContext context) => _context = context;

    public async Task<List<SeasonDto>> GetSeasonsByLeagueIdAsync(Guid leagueId)
    {
        return await _context.Seasons
            .AsNoTracking()
            .Where(s => s.LeagueId == leagueId)
            .Select(s => new SeasonDto
            {
                Id = s.Id,
                LeagueId = s.LeagueId,
                Name = s.Name,
                StartDate = s.StartDate,
                EndDate = s.EndDate,
                IsActive = s.IsActive,
                IsRegistrationOpen = s.IsRegistrationOpen,
                IsCompleted = s.IsCompleted
            })
            .ToListAsync();
    }

    public async Task<SeasonDto?> GetSeasonByIdAsync(Guid id)
    {
        var season = await _context.Seasons.AsNoTracking().FirstOrDefaultAsync(s => s.Id == id);
        if (season == null) return null;

        return new SeasonDto
        {
            Id = season.Id,
            LeagueId = season.LeagueId,
            Name = season.Name,
            StartDate = season.StartDate,
            EndDate = season.EndDate,
            IsActive = season.IsActive,
            IsRegistrationOpen = season.IsRegistrationOpen,
            IsCompleted = season.IsCompleted
        };
    }

    public async Task<Guid> CreateSeasonAsync(SeasonDto seasonDto)
    {
        var season = new Season
        {
            LeagueId = seasonDto.LeagueId,
            Name = seasonDto.Name,
            StartDate = seasonDto.StartDate,
            EndDate = seasonDto.EndDate,
            IsActive = seasonDto.IsActive,
            IsRegistrationOpen = seasonDto.IsRegistrationOpen,
            IsCompleted = seasonDto.IsCompleted
        };
        _context.Seasons.Add(season);
        await _context.SaveChangesAsync();
        return season.Id;
    }

    public async Task UpdateSeasonAsync(SeasonDto seasonDto)
    {
        var existing = await _context.Seasons.FindAsync(seasonDto.Id);
        if (existing == null) throw new KeyNotFoundException("Season not found.");

        existing.Name = seasonDto.Name;
        existing.StartDate = seasonDto.StartDate;
        existing.EndDate = seasonDto.EndDate;
        existing.IsActive = seasonDto.IsActive;
        existing.IsRegistrationOpen = seasonDto.IsRegistrationOpen;
        existing.IsCompleted = seasonDto.IsCompleted;

        await _context.SaveChangesAsync();
    }

    public async Task DeleteSeasonAsync(Guid id)
    {
        var existing = await _context.Seasons.FindAsync(id);
        if (existing != null)
        {
            _context.Seasons.Remove(existing);
            await _context.SaveChangesAsync();
        }
    }
}