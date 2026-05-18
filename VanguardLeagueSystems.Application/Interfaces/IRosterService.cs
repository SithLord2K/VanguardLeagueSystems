using System;
using System.Collections.Generic;
using System.Threading.Tasks;
using VanguardLeagueSystems.Application.DTOs;

namespace VanguardLeagueSystems.Application.Interfaces;

public interface IRosterService
{
    Task<List<RosterSlotDto>> GetRosterByTeamIdAsync(Guid teamId);
    Task<Guid> AssignPlayerToTeamAsync(RosterSlotDto rosterDto);
    Task UpdateRosterSlotAsync(RosterSlotDto rosterDto);
    Task RemovePlayerFromTeamAsync(Guid rosterSlotId);
}