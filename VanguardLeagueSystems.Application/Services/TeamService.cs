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

public class TeamService : ITeamService
{
    private readonly VanguardDbContext _context;

    public TeamService(VanguardDbContext context) => _context = context;

    public async Task<List<TeamDto>> GetTeamsBySeasonIdAsync(Guid seasonId)
    {
        return await _context.Teams
            .AsNoTracking()
            .Where(t => t.SeasonId == seasonId)
            .Select(t => new TeamDto
            {
                Id = t.Id,
                SeasonId = t.SeasonId,
                Name = t.Name,
                PrimaryColorHex = t.PrimaryColorHex,
                SecondaryColorHex = t.SecondaryColorHex
            })
            .ToListAsync();
    }

    public async Task<TeamDto?> GetTeamByIdAsync(Guid id)
    {
        var team = await _context.Teams.AsNoTracking().FirstOrDefaultAsync(t => t.Id == id);
        if (team == null) return null;

        return new TeamDto
        {
            Id = team.Id,
            SeasonId = team.SeasonId,
            Name = team.Name,
            PrimaryColorHex = team.PrimaryColorHex,
            SecondaryColorHex = team.SecondaryColorHex
        };
    }

    public async Task<Guid> CreateTeamAsync(TeamDto teamDto)
    {
        var team = new Team
        {
            SeasonId = teamDto.SeasonId,
            Name = teamDto.Name,
            PrimaryColorHex = teamDto.PrimaryColorHex,
            SecondaryColorHex = teamDto.SecondaryColorHex
        };
        _context.Teams.Add(team);
        await _context.SaveChangesAsync();
        return team.Id;
    }

    public async Task UpdateTeamAsync(TeamDto teamDto)
    {
        var existing = await _context.Teams.FindAsync(teamDto.Id);
        if (existing == null) throw new KeyNotFoundException("Team not found.");

        existing.Name = teamDto.Name;
        existing.PrimaryColorHex = teamDto.PrimaryColorHex;
        existing.SecondaryColorHex = teamDto.SecondaryColorHex;

        await _context.SaveChangesAsync();
    }

    public async Task DeleteTeamAsync(Guid id)
    {
        var existing = await _context.Teams.FindAsync(id);
        if (existing != null)
        {
            _context.Teams.Remove(existing);
            await _context.SaveChangesAsync();
        }
    }
}