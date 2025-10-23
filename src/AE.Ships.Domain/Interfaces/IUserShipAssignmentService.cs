using AE.Ships.Domain.DTOs;

namespace AE.Ships.Domain.Interfaces;

public interface IUserShipAssignmentService
{
    Task<UserShipAssignmentDto> AssignShipToUserAsync(AssignShipToUserDto request);
    Task<bool> UnassignShipFromUserAsync(UnassignShipFromUserDto request);
    Task<IEnumerable<UserShipAssignmentDto>> GetUserShipAssignmentsAsync(int? userId = null, string? shipCode = null);
}
