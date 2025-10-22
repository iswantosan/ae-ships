using AE.Ships.Domain.DTOs;

namespace AE.Ships.Domain.Interfaces;

public interface IUserService
{
    Task<IEnumerable<UserDto>> GetAllUsersAsync();
    Task<UserDto?> GetUserByIdAsync(int userId);
    Task<UserDto> CreateUserAsync(CreateUserDto createUserDto);
    Task<UserDto> UpdateUserAsync(UpdateUserDto updateUserDto);
    Task<bool> DeleteUserAsync(int userId);
}
