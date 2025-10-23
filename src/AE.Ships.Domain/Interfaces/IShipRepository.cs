using AE.Ships.Domain.Entities;

namespace AE.Ships.Domain.Interfaces;

public interface IShipRepository
{
    Task<IEnumerable<Ship>> GetAllAsync();
    Task<Ship?> GetByCodeAsync(string shipCode);
    Task<Ship> CreateAsync(Ship ship);
    Task<Ship> UpdateAsync(Ship ship);
    Task<bool> DeleteAsync(string shipCode);
    Task<bool> ExistsAsync(string shipCode);
    Task<IEnumerable<Ship>> GetByStatusAsync(string status);
    Task<IEnumerable<Ship>> GetByUserAsync(int userId);
}


