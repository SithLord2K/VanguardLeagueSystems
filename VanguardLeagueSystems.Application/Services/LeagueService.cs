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

/// <summary>
/// Implements business logic and database interactions for Leagues.
/// </summary>
public class LeagueService : ILeagueService
{
    private readonly VanguardDbContext _context;

    public LeagueService(VanguardDbContext context)
    {
        _context = context;
    }

    public async Task<List<LeagueDto>> GetAllLeaguesAsync()
    {
        // AsNoTracking is critical for read-only UI lists to prevent memory bloat
        return await _context.Leagues
            .AsNoTracking()
            .Select(l => new LeagueDto
            {
                Id = l.Id,
                Name = l.Name,
                Description = l.Description,
                Sport = l.Sport,
                IsActive = l.IsActive,
                Configuration = l.Configuration
            })
            .ToListAsync();
    }

    public async Task<LeagueDto?> GetLeagueByIdAsync(Guid id)
    {
        var league = await _context.Leagues
            .AsNoTracking()
            .FirstOrDefaultAsync(l => l.Id == id);

        if (league == null) return null;

        return new LeagueDto
        {
            Id = league.Id,
            Name = league.Name,
            Description = league.Description,
            Sport = league.Sport,
            IsActive = league.IsActive,
            Configuration = league.Configuration
        };
    }

    public async Task<Guid> CreateLeagueAsync(LeagueDto leagueDto)
    {
        var newLeague = new League
        {
            Name = leagueDto.Name,
            Description = leagueDto.Description,
            Sport = leagueDto.Sport,
            IsActive = leagueDto.IsActive,
            Configuration = leagueDto.Configuration
        };

        _context.Leagues.Add(newLeague);
        await _context.SaveChangesAsync();

        return newLeague.Id;
    }

    public async Task UpdateLeagueAsync(LeagueDto leagueDto)
    {
        var existingLeague = await _context.Leagues.FindAsync(leagueDto.Id);

        if (existingLeague == null)
            throw new KeyNotFoundException($"League with ID {leagueDto.Id} not found.");

        existingLeague.Name = leagueDto.Name;
        existingLeague.Description = leagueDto.Description;
        existingLeague.Sport = leagueDto.Sport;
        existingLeague.IsActive = leagueDto.IsActive;
        existingLeague.Configuration = leagueDto.Configuration;

        await _context.SaveChangesAsync();
    }

    public async Task DeleteLeagueAsync(Guid id)
    {
        var existingLeague = await _context.Leagues.FindAsync(id);

        if (existingLeague != null)
        {
            _context.Leagues.Remove(existingLeague);
            // SaveChangesAsync will automatically intercept this and convert it to a Soft Delete
            await _context.SaveChangesAsync();
        }
    }
}