using AE.Ships.Domain.DTOs;

namespace AE.Ships.Domain.Interfaces;

public interface IShipService
{
    Task<IEnumerable<ShipDto>> GetAllShipsAsync();
    Task<ShipDto?> GetShipByCodeAsync(string shipCode);
    Task<ShipDto> CreateShipAsync(CreateShipDto createShipDto);
    Task<ShipDto> UpdateShipAsync(UpdateShipDto updateShipDto);
    Task<bool> DeleteShipAsync(string shipCode);
    Task<IEnumerable<ShipDto>> GetShipsByStatusAsync(string status);
    Task<IEnumerable<ShipDto>> GetShipsByUserAsync(int userId);
}

