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

public class RosterService : IRosterService
{
    private readonly VanguardDbContext _context;

    public RosterService(VanguardDbContext context) => _context = context;

    public async Task<List<RosterSlotDto>> GetRosterByTeamIdAsync(Guid teamId)
    {
        return await _context.RosterSlots
            .AsNoTracking()
            .Include(r => r.Player) // Fetch player data for the UI
            .Where(r => r.TeamId == teamId)
            .Select(r => new RosterSlotDto
            {
                Id = r.Id,
                TeamId = r.TeamId,
                PlayerId = r.PlayerId,
                PlayerName = $"{r.Player!.FirstName} {r.Player.LastName}",
                JerseyNumber = r.JerseyNumber,
                Position = r.Position,
                IsCaptain = r.IsCaptain
            })
            .ToListAsync();
    }

    public async Task<Guid> AssignPlayerToTeamAsync(RosterSlotDto rosterDto)
    {
        var slot = new RosterSlot
        {
            TeamId = rosterDto.TeamId,
            PlayerId = rosterDto.PlayerId,
            JerseyNumber = rosterDto.JerseyNumber,
            Position = rosterDto.Position,
            IsCaptain = rosterDto.IsCaptain
        };
        _context.RosterSlots.Add(slot);
        await _context.SaveChangesAsync();
        return slot.Id;
    }

    public async Task UpdateRosterSlotAsync(RosterSlotDto rosterDto)
    {
        var existing = await _context.RosterSlots.FindAsync(rosterDto.Id);
        if (existing == null) throw new KeyNotFoundException("Roster slot not found.");

        existing.JerseyNumber = rosterDto.JerseyNumber;
        existing.Position = rosterDto.Position;
        existing.IsCaptain = rosterDto.IsCaptain;

        await _context.SaveChangesAsync();
    }

    public async Task RemovePlayerFromTeamAsync(Guid rosterSlotId)
    {
        var existing = await _context.RosterSlots.FindAsync(rosterSlotId);
        if (existing != null)
        {
            _context.RosterSlots.Remove(existing);
            await _context.SaveChangesAsync();
        }
    }
}