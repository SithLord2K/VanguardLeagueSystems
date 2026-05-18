using System;
using System.Collections.Generic;
using System.Threading.Tasks;
using VanguardLeagueSystems.Application.DTOs;

namespace VanguardLeagueSystems.Application.Interfaces;

public interface IPlayerService
{
    Task<List<PlayerDto>> GetAllPlayersAsync();
    Task<PlayerDto?> GetPlayerByIdAsync(Guid id);
    Task<Guid> CreatePlayerAsync(PlayerDto playerDto);
    Task UpdatePlayerAsync(PlayerDto playerDto);
    Task DeletePlayerAsync(Guid id);
}