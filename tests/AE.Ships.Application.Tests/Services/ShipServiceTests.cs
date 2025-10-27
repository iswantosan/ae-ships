using AE.Ships.Application.Services;
using AE.Ships.Domain.DTOs;
using AE.Ships.Domain.Entities;
using AE.Ships.Domain.Interfaces;
using Moq;
using Xunit;

namespace AE.Ships.Application.Tests.Services;

public class ShipServiceTests
{
    private readonly Mock<IShipRepository> _mockRepository;
    private readonly ShipService _shipService;

    public ShipServiceTests()
    {
        _mockRepository = new Mock<IShipRepository>();
        _shipService = new ShipService(_mockRepository.Object);
    }

    [Fact]
    public async Task GetAllShipsAsync_ShouldReturnAllShips()
    {
        var ships = new List<Ship>
        {
            new Ship { Code = "SHIP01", Name = "Flying Dutchman", FiscalYear = "0112", Status = "Active" },
            new Ship { Code = "SHIP02", Name = "Thousand Sunny", FiscalYear = "0403", Status = "Active" }
        };

        _mockRepository.Setup(x => x.GetAllAsync()).ReturnsAsync(ships);

        var result = await _shipService.GetAllShipsAsync();

        Assert.NotNull(result);
        Assert.Equal(2, result.Count());
        
        var shipList = result.ToList();
        Assert.Equal("SHIP01", shipList[0].Code);
        Assert.Equal("Flying Dutchman", shipList[0].Name);
        Assert.Equal("0112", shipList[0].FiscalYear);
        Assert.Equal("Active", shipList[0].Status);
    }

    [Fact]
    public async Task GetAllShipsAsync_WhenNoShips_ShouldReturnEmptyList()
    {
        _mockRepository.Setup(x => x.GetAllAsync()).ReturnsAsync(new List<Ship>());

        var result = await _shipService.GetAllShipsAsync();

        Assert.NotNull(result);
        Assert.Empty(result);
    }

    [Fact]
    public async Task GetShipByCodeAsync_WhenShipExists_ShouldReturnShip()
    {
        var ship = new Ship { Code = "SHIP01", Name = "Flying Dutchman", FiscalYear = "0112", Status = "Active" };
        _mockRepository.Setup(x => x.GetByCodeAsync("SHIP01")).ReturnsAsync(ship);

        var result = await _shipService.GetShipByCodeAsync("SHIP01");

        Assert.NotNull(result);
        Assert.Equal("SHIP01", result.Code);
        Assert.Equal("Flying Dutchman", result.Name);
        Assert.Equal("0112", result.FiscalYear);
        Assert.Equal("Active", result.Status);
    }

    [Fact]
    public async Task GetShipByCodeAsync_WhenShipDoesNotExist_ShouldReturnNull()
    {
        _mockRepository.Setup(x => x.GetByCodeAsync("INVALID")).ReturnsAsync((Ship?)null);

        var result = await _shipService.GetShipByCodeAsync("INVALID");

        Assert.Null(result);
    }

    [Fact]
    public async Task CreateShipAsync_ShouldCreateAndReturnShip()
    {
        var createShipDto = new CreateShipDto 
        { 
            Code = "SHIP03", 
            Name = "New Ship", 
            FiscalYear = "0703", 
            Status = "Active" 
        };
        var createdShip = new Ship 
        { 
            Code = "SHIP03", 
            Name = "New Ship", 
            FiscalYear = "0703", 
            Status = "Active" 
        };

        _mockRepository.Setup(x => x.CreateAsync(It.IsAny<Ship>())).ReturnsAsync(createdShip);

        var result = await _shipService.CreateShipAsync(createShipDto);

        Assert.NotNull(result);
        Assert.Equal("SHIP03", result.Code);
        Assert.Equal("New Ship", result.Name);
        Assert.Equal("0703", result.FiscalYear);
        Assert.Equal("Active", result.Status);

        _mockRepository.Verify(x => x.CreateAsync(It.Is<Ship>(s => 
            s.Code == "SHIP03" && 
            s.Name == "New Ship" && 
            s.FiscalYear == "0703" && 
            s.Status == "Active")), Times.Once);
    }

    [Fact]
    public async Task UpdateShipAsync_ShouldUpdateAndReturnShip()
    {
        var updateShipDto = new UpdateShipDto 
        { 
            Code = "SHIP01", 
            Name = "Updated Ship", 
            FiscalYear = "0112", 
            Status = "Inactive" 
        };
        var updatedShip = new Ship 
        { 
            Code = "SHIP01", 
            Name = "Updated Ship", 
            FiscalYear = "0112", 
            Status = "Inactive" 
        };

        _mockRepository.Setup(x => x.UpdateAsync(It.IsAny<Ship>())).ReturnsAsync(updatedShip);

        var result = await _shipService.UpdateShipAsync(updateShipDto);

        Assert.NotNull(result);
        Assert.Equal("SHIP01", result.Code);
        Assert.Equal("Updated Ship", result.Name);
        Assert.Equal("0112", result.FiscalYear);
        Assert.Equal("Inactive", result.Status);

        _mockRepository.Verify(x => x.UpdateAsync(It.Is<Ship>(s => 
            s.Code == "SHIP01" && 
            s.Name == "Updated Ship" && 
            s.FiscalYear == "0112" && 
            s.Status == "Inactive")), Times.Once);
    }

    [Fact]
    public async Task DeleteShipAsync_WhenShipExists_ShouldReturnTrue()
    {
        _mockRepository.Setup(x => x.DeleteAsync("SHIP01")).ReturnsAsync(true);

        var result = await _shipService.DeleteShipAsync("SHIP01");

        Assert.True(result);
        _mockRepository.Verify(x => x.DeleteAsync("SHIP01"), Times.Once);
    }

    [Fact]
    public async Task DeleteShipAsync_WhenShipDoesNotExist_ShouldReturnFalse()
    {
        _mockRepository.Setup(x => x.DeleteAsync("INVALID")).ReturnsAsync(false);

        var result = await _shipService.DeleteShipAsync("INVALID");

        Assert.False(result);
        _mockRepository.Verify(x => x.DeleteAsync("INVALID"), Times.Once);
    }

    [Fact]
    public async Task GetShipsByStatusAsync_ShouldReturnShipsWithStatus()
    {
        var ships = new List<Ship>
        {
            new Ship { Code = "SHIP01", Name = "Flying Dutchman", FiscalYear = "0112", Status = "Active" },
            new Ship { Code = "SHIP02", Name = "Thousand Sunny", FiscalYear = "0403", Status = "Active" }
        };

        _mockRepository.Setup(x => x.GetByStatusAsync("Active")).ReturnsAsync(ships);

        var result = await _shipService.GetShipsByStatusAsync("Active");

        Assert.NotNull(result);
        Assert.Equal(2, result.Count());
        
        var shipList = result.ToList();
        Assert.All(shipList, ship => Assert.Equal("Active", ship.Status));
    }

    [Fact]
    public async Task GetShipsByStatusAsync_WhenNoShipsWithStatus_ShouldReturnEmptyList()
    {
        _mockRepository.Setup(x => x.GetByStatusAsync("Inactive")).ReturnsAsync(new List<Ship>());

        var result = await _shipService.GetShipsByStatusAsync("Inactive");

        Assert.NotNull(result);
        Assert.Empty(result);
    }

    [Fact]
    public async Task GetShipsByUserAsync_ShouldReturnShipsForUser()
    {
        var ships = new List<Ship>
        {
            new Ship { Code = "SHIP01", Name = "Flying Dutchman", FiscalYear = "0112", Status = "Active" }
        };

        _mockRepository.Setup(x => x.GetByUserAsync(1)).ReturnsAsync(ships);

        var result = await _shipService.GetShipsByUserAsync(1);

        Assert.NotNull(result);
        Assert.Single(result);
        Assert.Equal("SHIP01", result.First().Code);
    }

    [Fact]
    public async Task GetShipsByUserAsync_WhenUserHasNoShips_ShouldReturnEmptyList()
    {
        _mockRepository.Setup(x => x.GetByUserAsync(999)).ReturnsAsync(new List<Ship>());

        var result = await _shipService.GetShipsByUserAsync(999);

        Assert.NotNull(result);
        Assert.Empty(result);
    }

    [Fact]
    public async Task CreateShipAsync_ShouldMapPropertiesCorrectly()
    {
        var createShipDto = new CreateShipDto 
        { 
            Code = "TEST01", 
            Name = "Test Ship", 
            FiscalYear = "0101", 
            Status = "Active" 
        };
        var createdShip = new Ship 
        { 
            Code = "TEST01", 
            Name = "Test Ship", 
            FiscalYear = "0101", 
            Status = "Active" 
        };

        _mockRepository.Setup(x => x.CreateAsync(It.IsAny<Ship>())).ReturnsAsync(createdShip);

        var result = await _shipService.CreateShipAsync(createShipDto);

        _mockRepository.Verify(x => x.CreateAsync(It.Is<Ship>(s => 
            s.Code == createShipDto.Code && 
            s.Name == createShipDto.Name && 
            s.FiscalYear == createShipDto.FiscalYear && 
            s.Status == createShipDto.Status)), Times.Once);
    }

    [Fact]
    public async Task UpdateShipAsync_ShouldMapPropertiesCorrectly()
    {
        var updateShipDto = new UpdateShipDto 
        { 
            Code = "UPDATE01", 
            Name = "Updated Test Ship", 
            FiscalYear = "1212", 
            Status = "Inactive" 
        };
        var updatedShip = new Ship 
        { 
            Code = "UPDATE01", 
            Name = "Updated Test Ship", 
            FiscalYear = "1212", 
            Status = "Inactive" 
        };

        _mockRepository.Setup(x => x.UpdateAsync(It.IsAny<Ship>())).ReturnsAsync(updatedShip);

        var result = await _shipService.UpdateShipAsync(updateShipDto);

        _mockRepository.Verify(x => x.UpdateAsync(It.Is<Ship>(s => 
            s.Code == updateShipDto.Code && 
            s.Name == updateShipDto.Name && 
            s.FiscalYear == updateShipDto.FiscalYear && 
            s.Status == updateShipDto.Status)), Times.Once);
    }
}


