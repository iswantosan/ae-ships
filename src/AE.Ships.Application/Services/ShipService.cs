using AE.Ships.Domain.DTOs;
using AE.Ships.Domain.Entities;
using AE.Ships.Domain.Interfaces;

namespace AE.Ships.Application.Services;

public class ShipService : IShipService
{
    private readonly IShipRepository _shipRepository;

    public ShipService(IShipRepository shipRepository)
    {
        _shipRepository = shipRepository;
    }

    public async Task<IEnumerable<ShipDto>> GetAllShipsAsync()
    {
        var ships = await _shipRepository.GetAllAsync();
        return ships.Select(MapToDto);
    }

    public async Task<ShipDto?> GetShipByCodeAsync(string shipCode)
    {
        var ship = await _shipRepository.GetByCodeAsync(shipCode);
        return ship != null ? MapToDto(ship) : null;
    }

    public async Task<ShipDto> CreateShipAsync(CreateShipDto createShipDto)
    {
        var ship = new Ship
        {
            Code = createShipDto.Code,
            Name = createShipDto.Name,
            FiscalYear = createShipDto.FiscalYear,
            Status = createShipDto.Status
        };

        var createdShip = await _shipRepository.CreateAsync(ship);
        return MapToDto(createdShip);
    }

    public async Task<ShipDto> UpdateShipAsync(UpdateShipDto updateShipDto)
    {
        var ship = new Ship
        {
            Code = updateShipDto.Code,
            Name = updateShipDto.Name,
            FiscalYear = updateShipDto.FiscalYear,
            Status = updateShipDto.Status
        };

        var updatedShip = await _shipRepository.UpdateAsync(ship);
        return MapToDto(updatedShip);
    }

    public async Task<bool> DeleteShipAsync(string shipCode)
    {
        return await _shipRepository.DeleteAsync(shipCode);
    }

    public async Task<IEnumerable<ShipDto>> GetShipsByStatusAsync(string status)
    {
        var ships = await _shipRepository.GetByStatusAsync(status);
        return ships.Select(MapToDto);
    }

    public async Task<IEnumerable<ShipDto>> GetShipsByUserAsync(int userId)
    {
        var ships = await _shipRepository.GetByUserAsync(userId);
        return ships.Select(MapToDto);
    }

    private static ShipDto MapToDto(Ship ship)
    {
        return new ShipDto
        {
            Code = ship.Code,
            Name = ship.Name,
            FiscalYear = ship.FiscalYear,
            Status = ship.Status
        };
    }
}

