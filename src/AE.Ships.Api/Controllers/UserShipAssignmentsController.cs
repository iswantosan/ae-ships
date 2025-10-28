using AE.Ships.Domain.DTOs;
using AE.Ships.Domain.Interfaces;
using Microsoft.AspNetCore.Mvc;
using Swashbuckle.AspNetCore.Annotations;

namespace AE.Ships.Api.Controllers;

/// <summary>
/// User ship assignments management controller
/// </summary>
[ApiController]
[Route("api/[controller]")]
[SwaggerTag("User ship assignments management endpoints for ship-user assignment operations")]
public class UserShipAssignmentsController : ControllerBase
{
    private readonly IUserShipAssignmentService _userShipAssignmentService;

    public UserShipAssignmentsController(IUserShipAssignmentService userShipAssignmentService)
    {
        _userShipAssignmentService = userShipAssignmentService;
    }

    /// <summary>
    /// Assign a ship to a user
    /// </summary>
    /// <param name="request">Assignment request data</param>
    /// <returns>Assignment details</returns>
    /// <response code="201">Ship assigned successfully</response>
    /// <response code="400">Invalid request data</response>
    /// <response code="500">Internal server error</response>
    [HttpPost("assign")]
    [SwaggerOperation(
        Summary = "Assign Ship to User",
        Description = "Assigns a ship to a user with the provided details",
        OperationId = "AssignShipToUser"
    )]
    [SwaggerResponse(201, "Ship assigned successfully", typeof(UserShipAssignmentDto))]
    [SwaggerResponse(400, "Invalid request data")]
    [SwaggerResponse(500, "Internal server error")]
    public async Task<ActionResult<UserShipAssignmentDto>> AssignShipToUser([FromBody] AssignShipToUserDto request)
    {
        try
        {
            if (request.UserId <= 0)
            {
                return BadRequest(new { message = "Valid UserId is required." });
            }

            if (string.IsNullOrWhiteSpace(request.ShipCode))
            {
                return BadRequest(new { message = "ShipCode is required." });
            }

            var assignment = await _userShipAssignmentService.AssignShipToUserAsync(request);
            return Ok(assignment);
        }
        catch (Exception ex)
        {
            return StatusCode(500, new { message = "An error occurred while assigning ship to user.", error = ex.Message });
        }
    }

    /// <summary>
    /// Unassign a ship from a user
    /// </summary>
    /// <param name="request">Unassignment request data</param>
    /// <returns>No content</returns>
    /// <response code="204">Ship unassigned successfully</response>
    /// <response code="400">Invalid request data</response>
    /// <response code="500">Internal server error</response>
    [HttpPost("unassign")]
    [SwaggerOperation(
        Summary = "Unassign Ship from User",
        Description = "Removes the assignment of a ship from a user",
        OperationId = "UnassignShipFromUser"
    )]
    [SwaggerResponse(204, "Ship unassigned successfully")]
    [SwaggerResponse(400, "Invalid request data")]
    [SwaggerResponse(500, "Internal server error")]
    public async Task<ActionResult> UnassignShipFromUser([FromBody] UnassignShipFromUserDto request)
    {
        try
        {
            if (request.UserId <= 0)
            {
                return BadRequest(new { message = "Valid UserId is required." });
            }

            if (string.IsNullOrWhiteSpace(request.ShipCode))
            {
                return BadRequest(new { message = "ShipCode is required." });
            }

            var unassigned = await _userShipAssignmentService.UnassignShipFromUserAsync(request);
            if (!unassigned)
            {
                return NotFound(new { message = "Ship assignment not found." });
            }

            return NoContent();
        }
        catch (Exception ex)
        {
            return StatusCode(500, new { message = "An error occurred while unassigning ship from user.", error = ex.Message });
        }
    }

    /// <summary>
    /// Get all user ship assignments
    /// </summary>
    /// <param name="pageNumber">Page number (default: 1)</param>
    /// <param name="pageSize">Page size (default: 10, max: 100)</param>
    /// <returns>Paginated list of assignments</returns>
    /// <response code="200">Assignments retrieved successfully</response>
    /// <response code="400">Invalid pagination parameters</response>
    /// <response code="500">Internal server error</response>
    [HttpGet]
    [SwaggerOperation(
        Summary = "Get All User Ship Assignments",
        Description = "Retrieves a paginated list of all user ship assignments",
        OperationId = "GetUserShipAssignments"
    )]
    [SwaggerResponse(200, "Assignments retrieved successfully", typeof(IEnumerable<UserShipAssignmentDto>))]
    [SwaggerResponse(400, "Invalid pagination parameters")]
    [SwaggerResponse(500, "Internal server error")]
    public async Task<ActionResult<IEnumerable<UserShipAssignmentDto>>> GetUserShipAssignments(
        [FromQuery] int? userId = null,
        [FromQuery] string? shipCode = null)
    {
        try
        {
            var assignments = await _userShipAssignmentService.GetUserShipAssignmentsAsync(userId, shipCode);
            return Ok(assignments);
        }
        catch (Exception ex)
        {
            return StatusCode(500, new { message = "An error occurred while retrieving user ship assignments.", error = ex.Message });
        }
    }

    /// <summary>
    /// Get assignments by user ID
    /// </summary>
    /// <param name="userId">User ID</param>
    /// <returns>List of assignments for the user</returns>
    /// <response code="200">Assignments retrieved successfully</response>
    /// <response code="400">Invalid user ID</response>
    /// <response code="500">Internal server error</response>
    [HttpGet("user/{userId}")]
    [SwaggerOperation(
        Summary = "Get Assignments by User",
        Description = "Retrieves all ship assignments for a specific user",
        OperationId = "GetAssignmentsByUser"
    )]
    [SwaggerResponse(200, "Assignments retrieved successfully", typeof(IEnumerable<UserShipAssignmentDto>))]
    [SwaggerResponse(400, "Invalid user ID")]
    [SwaggerResponse(500, "Internal server error")]
    public async Task<ActionResult<IEnumerable<UserShipAssignmentDto>>> GetAssignmentsByUser(int userId)
    {
        try
        {
            if (userId <= 0)
            {
                return BadRequest(new { message = "Valid UserId is required." });
            }

            var assignments = await _userShipAssignmentService.GetUserShipAssignmentsAsync(userId, null);
            return Ok(assignments);
        }
        catch (Exception ex)
        {
            return StatusCode(500, new { message = "An error occurred while retrieving assignments for user.", error = ex.Message });
        }
    }

    /// <summary>
    /// Get assignments by ship code
    /// </summary>
    /// <param name="shipCode">Ship code</param>
    /// <returns>List of assignments for the ship</returns>
    /// <response code="200">Assignments retrieved successfully</response>
    /// <response code="400">Invalid ship code</response>
    /// <response code="500">Internal server error</response>
    [HttpGet("ship/{shipCode}")]
    [SwaggerOperation(
        Summary = "Get Assignments by Ship",
        Description = "Retrieves all user assignments for a specific ship",
        OperationId = "GetAssignmentsByShip"
    )]
    [SwaggerResponse(200, "Assignments retrieved successfully", typeof(IEnumerable<UserShipAssignmentDto>))]
    [SwaggerResponse(400, "Invalid ship code")]
    [SwaggerResponse(500, "Internal server error")]
    public async Task<ActionResult<IEnumerable<UserShipAssignmentDto>>> GetAssignmentsByShip(string shipCode)
    {
        try
        {
            if (string.IsNullOrWhiteSpace(shipCode))
            {
                return BadRequest(new { message = "ShipCode is required." });
            }

            var assignments = await _userShipAssignmentService.GetUserShipAssignmentsAsync(null, shipCode);
            return Ok(assignments);
        }
        catch (Exception ex)
        {
            return StatusCode(500, new { message = "An error occurred while retrieving assignments for ship.", error = ex.Message });
        }
    }
}
