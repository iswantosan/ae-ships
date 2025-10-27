using AE.Ships.Application.Services;
using AE.Ships.Domain.DTOs;
using AE.Ships.Domain.Entities;
using AE.Ships.Domain.Interfaces;
using Moq;
using Xunit;

namespace AE.Ships.Application.Tests.Services;

public class UserServiceTests
{
    private readonly Mock<IUserRepository> _mockRepository;
    private readonly UserService _userService;

    public UserServiceTests()
    {
        _mockRepository = new Mock<IUserRepository>();
        _userService = new UserService(_mockRepository.Object);
    }

    [Fact]
    public async Task GetAllUsersAsync_ShouldReturnAllUsers()
    {
        var users = new List<User>
        {
            new User { UserId = 1, Name = "John Doe", Role = "Admin" },
            new User { UserId = 2, Name = "Jane Smith", Role = "User" }
        };

        _mockRepository.Setup(x => x.GetAllAsync()).ReturnsAsync(users);

        var result = await _userService.GetAllUsersAsync();

        Assert.NotNull(result);
        Assert.Equal(2, result.Count());
        
        var userList = result.ToList();
        Assert.Equal(1, userList[0].UserId);
        Assert.Equal("John Doe", userList[0].Name);
        Assert.Equal("Admin", userList[0].Role);
        Assert.Equal(2, userList[1].UserId);
        Assert.Equal("Jane Smith", userList[1].Name);
        Assert.Equal("User", userList[1].Role);
    }

    [Fact]
    public async Task GetAllUsersAsync_WhenNoUsers_ShouldReturnEmptyList()
    {
        _mockRepository.Setup(x => x.GetAllAsync()).ReturnsAsync(new List<User>());

        var result = await _userService.GetAllUsersAsync();

        Assert.NotNull(result);
        Assert.Empty(result);
    }

    [Fact]
    public async Task GetUserByIdAsync_WhenUserExists_ShouldReturnUser()
    {
        var user = new User { UserId = 1, Name = "John Doe", Role = "Admin" };
        _mockRepository.Setup(x => x.GetByIdAsync(1)).ReturnsAsync(user);

        var result = await _userService.GetUserByIdAsync(1);

        Assert.NotNull(result);
        Assert.Equal(1, result.UserId);
        Assert.Equal("John Doe", result.Name);
        Assert.Equal("Admin", result.Role);
    }

    [Fact]
    public async Task GetUserByIdAsync_WhenUserDoesNotExist_ShouldReturnNull()
    {
        _mockRepository.Setup(x => x.GetByIdAsync(999)).ReturnsAsync((User?)null);

        var result = await _userService.GetUserByIdAsync(999);

        Assert.Null(result);
    }

    [Fact]
    public async Task CreateUserAsync_ShouldCreateAndReturnUser()
    {
        var createUserDto = new CreateUserDto { Name = "John Doe", Role = "Admin" };
        var createdUser = new User { UserId = 1, Name = "John Doe", Role = "Admin" };

        _mockRepository.Setup(x => x.CreateAsync(It.IsAny<User>())).ReturnsAsync(createdUser);

        var result = await _userService.CreateUserAsync(createUserDto);

        Assert.NotNull(result);
        Assert.Equal(1, result.UserId);
        Assert.Equal("John Doe", result.Name);
        Assert.Equal("Admin", result.Role);

        _mockRepository.Verify(x => x.CreateAsync(It.Is<User>(u => 
            u.Name == "John Doe" && u.Role == "Admin")), Times.Once);
    }

    [Fact]
    public async Task UpdateUserAsync_ShouldUpdateAndReturnUser()
    {
        var updateUserDto = new UpdateUserDto { UserId = 1, Name = "John Updated", Role = "SuperAdmin" };
        var updatedUser = new User { UserId = 1, Name = "John Updated", Role = "SuperAdmin" };

        _mockRepository.Setup(x => x.UpdateAsync(It.IsAny<User>())).ReturnsAsync(updatedUser);

        var result = await _userService.UpdateUserAsync(updateUserDto);

        Assert.NotNull(result);
        Assert.Equal(1, result.UserId);
        Assert.Equal("John Updated", result.Name);
        Assert.Equal("SuperAdmin", result.Role);

        _mockRepository.Verify(x => x.UpdateAsync(It.Is<User>(u => 
            u.UserId == 1 && u.Name == "John Updated" && u.Role == "SuperAdmin")), Times.Once);
    }

    [Fact]
    public async Task DeleteUserAsync_WhenUserExists_ShouldReturnTrue()
    {
        _mockRepository.Setup(x => x.DeleteAsync(1)).ReturnsAsync(true);

        var result = await _userService.DeleteUserAsync(1);

        Assert.True(result);
        _mockRepository.Verify(x => x.DeleteAsync(1), Times.Once);
    }

    [Fact]
    public async Task DeleteUserAsync_WhenUserDoesNotExist_ShouldReturnFalse()
    {
        _mockRepository.Setup(x => x.DeleteAsync(999)).ReturnsAsync(false);

        var result = await _userService.DeleteUserAsync(999);

        Assert.False(result);
        _mockRepository.Verify(x => x.DeleteAsync(999), Times.Once);
    }

    [Fact]
    public async Task CreateUserAsync_ShouldMapPropertiesCorrectly()
    {
        var createUserDto = new CreateUserDto { Name = "Test User", Role = "Test Role" };
        var createdUser = new User { UserId = 5, Name = "Test User", Role = "Test Role" };

        _mockRepository.Setup(x => x.CreateAsync(It.IsAny<User>())).ReturnsAsync(createdUser);

        var result = await _userService.CreateUserAsync(createUserDto);

        _mockRepository.Verify(x => x.CreateAsync(It.Is<User>(u => 
            u.Name == createUserDto.Name && 
            u.Role == createUserDto.Role &&
            u.UserId == 0)), Times.Once);
    }

    [Fact]
    public async Task UpdateUserAsync_ShouldMapPropertiesCorrectly()
    {
        var updateUserDto = new UpdateUserDto { UserId = 3, Name = "Updated Name", Role = "Updated Role" };
        var updatedUser = new User { UserId = 3, Name = "Updated Name", Role = "Updated Role" };

        _mockRepository.Setup(x => x.UpdateAsync(It.IsAny<User>())).ReturnsAsync(updatedUser);

        var result = await _userService.UpdateUserAsync(updateUserDto);

        _mockRepository.Verify(x => x.UpdateAsync(It.Is<User>(u => 
            u.UserId == updateUserDto.UserId && 
            u.Name == updateUserDto.Name && 
            u.Role == updateUserDto.Role)), Times.Once);
    }
}


