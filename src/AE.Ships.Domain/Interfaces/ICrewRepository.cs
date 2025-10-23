using AE.Ships.Domain.DTOs;

namespace AE.Ships.Domain.Interfaces;

public interface ICrewRepository
{
    Task<IEnumerable<CrewListDto>> GetCrewListAsync(CrewListRequestDto request);
    Task<IEnumerable<CrewMemberHistoryDto>> GetCrewMemberHistoryAsync(string crewMemberId);
}
