using AE.Ships.Domain.DTOs;
using AE.Ships.Domain.Interfaces;
using Microsoft.AspNetCore.Mvc;
using Swashbuckle.AspNetCore.Annotations;

namespace AE.Ships.Api.Controllers;

/// <summary>
/// Users management controller
/// </summary>
[ApiController]
[Route("api/[controller]")]
[SwaggerTag("Users management endpoints for user operations")]
public class UsersController : ControllerBase
{
    private readonly IUserService _userService;

    public UsersController(IUserService userService)
    {
        _userService = userService;
    }

    /// <summary>
    /// Get all users
    /// </summary>
    /// <returns>List of all users</returns>
    /// <response code="200">Users retrieved successfully</response>
    /// <response code="500">Internal server error</response>
    [HttpGet]
    [SwaggerOperation(
        Summary = "Get All Users",
        Description = "Retrieves a list of all users in the system",
        OperationId = "GetAllUsers"
    )]
    [SwaggerResponse(200, "Users retrieved successfully", typeof(IEnumerable<UserDto>))]
    [SwaggerResponse(500, "Internal server error")]
    public async Task<ActionResult<IEnumerable<UserDto>>> GetAllUsers()
    {
        try
        {
            var users = await _userService.GetAllUsersAsync();
            return Ok(users);
        }
        catch (Exception ex)
        {
            return StatusCode(500, new { message = "An error occurred while retrieving users.", error = ex.Message });
        }
    }

    /// <summary>
    /// Get user by ID
    /// </summary>
    /// <param name="id">User ID</param>
    /// <returns>User details</returns>
    /// <response code="200">User found</response>
    /// <response code="404">User not found</response>
    /// <response code="500">Internal server error</response>
    [HttpGet("{id}")]
    [SwaggerOperation(
        Summary = "Get User by ID",
        Description = "Retrieves a specific user by their ID",
        OperationId = "GetUser"
    )]
    [SwaggerResponse(200, "User found", typeof(UserDto))]
    [SwaggerResponse(404, "User not found")]
    [SwaggerResponse(500, "Internal server error")]
    public async Task<ActionResult<UserDto>> GetUser(int id)
    {
        try
        {
            var user = await _userService.GetUserByIdAsync(id);
            if (user == null)
            {
                return NotFound(new { message = $"User with ID {id} not found." });
            }
            return Ok(user);
        }
        catch (Exception ex)
        {
            return StatusCode(500, new { message = "An error occurred while retrieving the user.", error = ex.Message });
        }
    }

    /// <summary>
    /// Create a new user
    /// </summary>
    /// <param name="createUserDto">User creation data</param>
    /// <returns>Created user details</returns>
    /// <response code="201">User created successfully</response>
    /// <response code="400">Invalid request data</response>
    /// <response code="500">Internal server error</response>
    [HttpPost]
    [SwaggerOperation(
        Summary = "Create User",
        Description = "Creates a new user with the provided details",
        OperationId = "CreateUser"
    )]
    [SwaggerResponse(201, "User created successfully", typeof(UserDto))]
    [SwaggerResponse(400, "Invalid request data")]
    [SwaggerResponse(500, "Internal server error")]
    public async Task<ActionResult<UserDto>> CreateUser([FromBody] CreateUserDto createUserDto)
    {
        try
        {
            if (string.IsNullOrWhiteSpace(createUserDto.Name))
            {
                return BadRequest(new { message = "User name is required." });
            }

            if (string.IsNullOrWhiteSpace(createUserDto.Role))
            {
                return BadRequest(new { message = "User role is required." });
            }

            var user = await _userService.CreateUserAsync(createUserDto);
            return CreatedAtAction(nameof(GetUser), new { id = user.UserId }, user);
        }
        catch (Exception ex)
        {
            return StatusCode(500, new { message = "An error occurred while creating the user.", error = ex.Message });
        }
    }

    /// <summary>
    /// Update an existing user
    /// </summary>
    /// <param name="id">User ID</param>
    /// <param name="updateUserDto">Updated user data</param>
    /// <returns>Updated user details</returns>
    /// <response code="200">User updated successfully</response>
    /// <response code="400">Invalid request data</response>
    /// <response code="500">Internal server error</response>
    [HttpPut("{id}")]
    [SwaggerOperation(
        Summary = "Update User",
        Description = "Updates an existing user with new details",
        OperationId = "UpdateUser"
    )]
    [SwaggerResponse(200, "User updated successfully", typeof(UserDto))]
    [SwaggerResponse(400, "Invalid request data")]
    [SwaggerResponse(500, "Internal server error")]
    public async Task<ActionResult<UserDto>> UpdateUser(int id, [FromBody] UpdateUserDto updateUserDto)
    {
        try
        {
            if (id != updateUserDto.UserId)
            {
                return BadRequest(new { message = "User ID mismatch." });
            }

            if (string.IsNullOrWhiteSpace(updateUserDto.Name))
            {
                return BadRequest(new { message = "User name is required." });
            }

            if (string.IsNullOrWhiteSpace(updateUserDto.Role))
            {
                return BadRequest(new { message = "User role is required." });
            }

            var user = await _userService.UpdateUserAsync(updateUserDto);
            return Ok(user);
        }
        catch (Exception ex)
        {
            return StatusCode(500, new { message = "An error occurred while updating the user.", error = ex.Message });
        }
    }

    /// <summary>
    /// Delete a user
    /// </summary>
    /// <param name="id">User ID</param>
    /// <returns>No content</returns>
    /// <response code="204">User deleted successfully</response>
    /// <response code="404">User not found</response>
    /// <response code="500">Internal server error</response>
    [HttpDelete("{id}")]
    [SwaggerOperation(
        Summary = "Delete User",
        Description = "Deletes a user by their ID",
        OperationId = "DeleteUser"
    )]
    [SwaggerResponse(204, "User deleted successfully")]
    [SwaggerResponse(404, "User not found")]
    [SwaggerResponse(500, "Internal server error")]
    public async Task<ActionResult> DeleteUser(int id)
    {
        try
        {
            var deleted = await _userService.DeleteUserAsync(id);
            if (!deleted)
            {
                return NotFound(new { message = $"User with ID {id} not found." });
            }
            return NoContent();
        }
        catch (Exception ex)
        {
            return StatusCode(500, new { message = "An error occurred while deleting the user.", error = ex.Message });
        }
    }
}
