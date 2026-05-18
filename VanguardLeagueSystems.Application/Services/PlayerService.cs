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

public class PlayerService : IPlayerService
{
    private readonly VanguardDbContext _context;

    public PlayerService(VanguardDbContext context) => _context = context;

    public async Task<List<PlayerDto>> GetAllPlayersAsync()
    {
        return await _context.Players
            .AsNoTracking()
            .Select(p => new PlayerDto
            {
                Id = p.Id,
                FirstName = p.FirstName,
                LastName = p.LastName,
                ContactEmail = p.ContactEmail,
                DateOfBirth = p.DateOfBirth
            })
            .ToListAsync();
    }

    public async Task<PlayerDto?> GetPlayerByIdAsync(Guid id)
    {
        var p = await _context.Players.AsNoTracking().FirstOrDefaultAsync(x => x.Id == id);
        if (p == null) return null;

        return new PlayerDto
        {
            Id = p.Id,
            FirstName = p.FirstName,
            LastName = p.LastName,
            ContactEmail = p.ContactEmail,
            DateOfBirth = p.DateOfBirth
        };
    }

    public async Task<Guid> CreatePlayerAsync(PlayerDto playerDto)
    {
        var player = new Player
        {
            FirstName = playerDto.FirstName,
            LastName = playerDto.LastName,
            ContactEmail = playerDto.ContactEmail,
            DateOfBirth = playerDto.DateOfBirth
        };
        _context.Players.Add(player);
        await _context.SaveChangesAsync();
        return player.Id;
    }

    public async Task UpdatePlayerAsync(PlayerDto playerDto)
    {
        var existing = await _context.Players.FindAsync(playerDto.Id);
        if (existing == null) throw new KeyNotFoundException("Player not found.");

        existing.FirstName = playerDto.FirstName;
        existing.LastName = playerDto.LastName;
        existing.ContactEmail = playerDto.ContactEmail;
        existing.DateOfBirth = playerDto.DateOfBirth;

        await _context.SaveChangesAsync();
    }

    public async Task DeletePlayerAsync(Guid id)
    {
        var existing = await _context.Players.FindAsync(id);
        if (existing != null)
        {
            _context.Players.Remove(existing);
            await _context.SaveChangesAsync();
        }
    }
}