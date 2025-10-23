using AE.Ships.Domain.DTOs;
using AE.Ships.Domain.Interfaces;

namespace AE.Ships.Application.Services;

public class UserShipAssignmentService : IUserShipAssignmentService
{
    private readonly IUserShipAssignmentRepository _userShipAssignmentRepository;

    public UserShipAssignmentService(IUserShipAssignmentRepository userShipAssignmentRepository)
    {
        _userShipAssignmentRepository = userShipAssignmentRepository;
    }

    public async Task<UserShipAssignmentDto> AssignShipToUserAsync(AssignShipToUserDto request)
    {
        return await _userShipAssignmentRepository.AssignShipToUserAsync(request.UserId, request.ShipCode);
    }

    public async Task<bool> UnassignShipFromUserAsync(UnassignShipFromUserDto request)
    {
        return await _userShipAssignmentRepository.UnassignShipFromUserAsync(request.UserId, request.ShipCode);
    }

    public async Task<IEnumerable<UserShipAssignmentDto>> GetUserShipAssignmentsAsync(int? userId = null, string? shipCode = null)
    {
        return await _userShipAssignmentRepository.GetUserShipAssignmentsAsync(userId, shipCode);
    }
}
