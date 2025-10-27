using AE.Ships.Application.Services;
using AE.Ships.Domain.DTOs;
using AE.Ships.Domain.Interfaces;
using Moq;
using Xunit;

namespace AE.Ships.Application.Tests.Services;

public class UserShipAssignmentServiceTests
{
    private readonly Mock<IUserShipAssignmentRepository> _mockRepository;
    private readonly UserShipAssignmentService _userShipAssignmentService;

    public UserShipAssignmentServiceTests()
    {
        _mockRepository = new Mock<IUserShipAssignmentRepository>();
        _userShipAssignmentService = new UserShipAssignmentService(_mockRepository.Object);
    }

    [Fact]
    public async Task AssignShipToUserAsync_ShouldReturnAssignment()
    {
        var request = new AssignShipToUserDto
        {
            UserId = 1,
            ShipCode = "SHIP01"
        };

        var assignment = new UserShipAssignmentDto
        {
            UserId = 1,
            ShipCode = "SHIP01",
            AssignedDate = DateTime.Parse("2025-01-15"),
            UserName = "John Doe",
            UserRole = "Admin",
            ShipName = "Flying Dutchman",
            FiscalYear = "0112",
            ShipStatus = "Active"
        };

        _mockRepository.Setup(x => x.AssignShipToUserAsync(request.UserId, request.ShipCode))
            .ReturnsAsync(assignment);

        var result = await _userShipAssignmentService.AssignShipToUserAsync(request);

        Assert.NotNull(result);
        Assert.Equal(1, result.UserId);
        Assert.Equal("SHIP01", result.ShipCode);
        Assert.Equal("John Doe", result.UserName);
        Assert.Equal("Admin", result.UserRole);
        Assert.Equal("Flying Dutchman", result.ShipName);
        Assert.Equal("0112", result.FiscalYear);
        Assert.Equal("Active", result.ShipStatus);

        _mockRepository.Verify(x => x.AssignShipToUserAsync(1, "SHIP01"), Times.Once);
    }

    [Fact]
    public async Task UnassignShipFromUserAsync_ShouldReturnTrue()
    {
        var request = new UnassignShipFromUserDto
        {
            UserId = 1,
            ShipCode = "SHIP01"
        };

        _mockRepository.Setup(x => x.UnassignShipFromUserAsync(request.UserId, request.ShipCode))
            .ReturnsAsync(true);

        var result = await _userShipAssignmentService.UnassignShipFromUserAsync(request);

        Assert.True(result);
        _mockRepository.Verify(x => x.UnassignShipFromUserAsync(1, "SHIP01"), Times.Once);
    }

    [Fact]
    public async Task UnassignShipFromUserAsync_WhenAssignmentDoesNotExist_ShouldReturnFalse()
    {
        var request = new UnassignShipFromUserDto
        {
            UserId = 999,
            ShipCode = "INVALID"
        };

        _mockRepository.Setup(x => x.UnassignShipFromUserAsync(request.UserId, request.ShipCode))
            .ReturnsAsync(false);

        var result = await _userShipAssignmentService.UnassignShipFromUserAsync(request);

        Assert.False(result);
        _mockRepository.Verify(x => x.UnassignShipFromUserAsync(999, "INVALID"), Times.Once);
    }

    [Fact]
    public async Task GetUserShipAssignmentsAsync_WithUserId_ShouldReturnAssignmentsForUser()
    {
        var userId = 1;
        var assignments = new List<UserShipAssignmentDto>
        {
            new UserShipAssignmentDto
            {
                UserId = 1,
                ShipCode = "SHIP01",
                AssignedDate = DateTime.Parse("2025-01-15"),
                UserName = "John Doe",
                UserRole = "Admin",
                ShipName = "Flying Dutchman",
                FiscalYear = "0112",
                ShipStatus = "Active"
            },
            new UserShipAssignmentDto
            {
                UserId = 1,
                ShipCode = "SHIP02",
                AssignedDate = DateTime.Parse("2025-01-20"),
                UserName = "John Doe",
                UserRole = "Admin",
                ShipName = "Thousand Sunny",
                FiscalYear = "0403",
                ShipStatus = "Active"
            }
        };

        _mockRepository.Setup(x => x.GetUserShipAssignmentsAsync(userId, null))
            .ReturnsAsync(assignments);

        var result = await _userShipAssignmentService.GetUserShipAssignmentsAsync(userId, null);

        Assert.NotNull(result);
        Assert.Equal(2, result.Count());
        
        var assignmentArray = result.ToArray();
        Assert.Equal("SHIP01", assignmentArray[0].ShipCode);
        Assert.Equal("Flying Dutchman", assignmentArray[0].ShipName);
        Assert.Equal("SHIP02", assignmentArray[1].ShipCode);
        Assert.Equal("Thousand Sunny", assignmentArray[1].ShipName);

        _mockRepository.Verify(x => x.GetUserShipAssignmentsAsync(1, null), Times.Once);
    }

    [Fact]
    public async Task GetUserShipAssignmentsAsync_WithShipCode_ShouldReturnAssignmentsForShip()
    {
        var shipCode = "SHIP01";
        var assignments = new List<UserShipAssignmentDto>
        {
            new UserShipAssignmentDto
            {
                UserId = 1,
                ShipCode = "SHIP01",
                AssignedDate = DateTime.Parse("2025-01-15"),
                UserName = "John Doe",
                UserRole = "Admin",
                ShipName = "Flying Dutchman",
                FiscalYear = "0112",
                ShipStatus = "Active"
            },
            new UserShipAssignmentDto
            {
                UserId = 2,
                ShipCode = "SHIP01",
                AssignedDate = DateTime.Parse("2025-01-16"),
                UserName = "Jane Smith",
                UserRole = "User",
                ShipName = "Flying Dutchman",
                FiscalYear = "0112",
                ShipStatus = "Active"
            }
        };

        _mockRepository.Setup(x => x.GetUserShipAssignmentsAsync(null, shipCode))
            .ReturnsAsync(assignments);

        var result = await _userShipAssignmentService.GetUserShipAssignmentsAsync(null, shipCode);

        Assert.NotNull(result);
        Assert.Equal(2, result.Count());
        
        var assignmentArray = result.ToArray();
        Assert.Equal(1, assignmentArray[0].UserId);
        Assert.Equal("John Doe", assignmentArray[0].UserName);
        Assert.Equal(2, assignmentArray[1].UserId);
        Assert.Equal("Jane Smith", assignmentArray[1].UserName);

        _mockRepository.Verify(x => x.GetUserShipAssignmentsAsync(null, "SHIP01"), Times.Once);
    }

    [Fact]
    public async Task GetUserShipAssignmentsAsync_WithBothParameters_ShouldReturnFilteredAssignments()
    {
        var userId = 1;
        var shipCode = "SHIP01";
        var assignments = new List<UserShipAssignmentDto>
        {
            new UserShipAssignmentDto
            {
                UserId = 1,
                ShipCode = "SHIP01",
                AssignedDate = DateTime.Parse("2025-01-15"),
                UserName = "John Doe",
                UserRole = "Admin",
                ShipName = "Flying Dutchman",
                FiscalYear = "0112",
                ShipStatus = "Active"
            }
        };

        _mockRepository.Setup(x => x.GetUserShipAssignmentsAsync(userId, shipCode))
            .ReturnsAsync(assignments);

        var result = await _userShipAssignmentService.GetUserShipAssignmentsAsync(userId, shipCode);

        Assert.NotNull(result);
        Assert.Single(result);
        Assert.Equal(1, result.First().UserId);
        Assert.Equal("SHIP01", result.First().ShipCode);

        _mockRepository.Verify(x => x.GetUserShipAssignmentsAsync(1, "SHIP01"), Times.Once);
    }

    [Fact]
    public async Task GetUserShipAssignmentsAsync_WithNoParameters_ShouldReturnAllAssignments()
    {
        var assignments = new List<UserShipAssignmentDto>
        {
            new UserShipAssignmentDto
            {
                UserId = 1,
                ShipCode = "SHIP01",
                AssignedDate = DateTime.Parse("2025-01-15"),
                UserName = "John Doe",
                UserRole = "Admin",
                ShipName = "Flying Dutchman",
                FiscalYear = "0112",
                ShipStatus = "Active"
            },
            new UserShipAssignmentDto
            {
                UserId = 2,
                ShipCode = "SHIP02",
                AssignedDate = DateTime.Parse("2025-01-20"),
                UserName = "Jane Smith",
                UserRole = "User",
                ShipName = "Thousand Sunny",
                FiscalYear = "0403",
                ShipStatus = "Active"
            }
        };

        _mockRepository.Setup(x => x.GetUserShipAssignmentsAsync(null, null))
            .ReturnsAsync(assignments);

        var result = await _userShipAssignmentService.GetUserShipAssignmentsAsync(null, null);

        Assert.NotNull(result);
        Assert.Equal(2, result.Count());

        _mockRepository.Verify(x => x.GetUserShipAssignmentsAsync(null, null), Times.Once);
    }

    [Fact]
    public async Task GetUserShipAssignmentsAsync_WhenNoAssignments_ShouldReturnEmptyList()
    {
        _mockRepository.Setup(x => x.GetUserShipAssignmentsAsync(999, null))
            .ReturnsAsync(new List<UserShipAssignmentDto>());

        var result = await _userShipAssignmentService.GetUserShipAssignmentsAsync(999, null);

        Assert.NotNull(result);
        Assert.Empty(result);
    }

    [Fact]
    public async Task AssignShipToUserAsync_ShouldPassParametersCorrectly()
    {
        var request = new AssignShipToUserDto
        {
            UserId = 5,
            ShipCode = "TEST01"
        };

        var assignment = new UserShipAssignmentDto
        {
            UserId = 5,
            ShipCode = "TEST01",
            AssignedDate = DateTime.Now,
            UserName = "Test User",
            UserRole = "Test Role",
            ShipName = "Test Ship",
            FiscalYear = "0101",
            ShipStatus = "Active"
        };

        _mockRepository.Setup(x => x.AssignShipToUserAsync(It.IsAny<int>(), It.IsAny<string>()))
            .ReturnsAsync(assignment);

        var result = await _userShipAssignmentService.AssignShipToUserAsync(request);

        _mockRepository.Verify(x => x.AssignShipToUserAsync(5, "TEST01"), Times.Once);
    }
}


