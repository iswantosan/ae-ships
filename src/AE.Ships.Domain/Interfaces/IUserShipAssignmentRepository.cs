using AE.Ships.Domain.DTOs;

namespace AE.Ships.Domain.Interfaces;

public interface IUserShipAssignmentRepository
{
    Task<UserShipAssignmentDto> AssignShipToUserAsync(int userId, string shipCode);
    Task<bool> UnassignShipFromUserAsync(int userId, string shipCode);
    Task<IEnumerable<UserShipAssignmentDto>> GetUserShipAssignmentsAsync(int? userId = null, string? shipCode = null);
}
