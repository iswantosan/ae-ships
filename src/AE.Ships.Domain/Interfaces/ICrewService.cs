using AE.Ships.Domain.DTOs;

namespace AE.Ships.Domain.Interfaces;

public interface ICrewService
{
    Task<IEnumerable<CrewListDto>> GetCrewListForShipAsync(string shipCode, CrewListRequestDto request);
    Task<IEnumerable<CrewMemberHistoryDto>> GetCrewMemberHistoryAsync(string crewMemberId);
}
