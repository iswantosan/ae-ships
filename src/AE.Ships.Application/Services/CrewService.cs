using AE.Ships.Domain.DTOs;
using AE.Ships.Domain.Interfaces;

namespace AE.Ships.Application.Services;

public class CrewService : ICrewService
{
    private readonly ICrewRepository _crewRepository;

    public CrewService(ICrewRepository crewRepository)
    {
        _crewRepository = crewRepository;
    }

    public async Task<IEnumerable<CrewListDto>> GetCrewListAsync(CrewListRequestDto request)
    {
        return await _crewRepository.GetCrewListAsync(request);
    }

    public async Task<IEnumerable<CrewMemberHistoryDto>> GetCrewMemberHistoryAsync(string crewMemberId)
    {
        return await _crewRepository.GetCrewMemberHistoryAsync(crewMemberId);
    }
}
