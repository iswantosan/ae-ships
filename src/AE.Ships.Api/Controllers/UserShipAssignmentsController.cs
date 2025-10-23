using AE.Ships.Domain.DTOs;
using AE.Ships.Domain.Interfaces;
using Microsoft.AspNetCore.Mvc;

namespace AE.Ships.Api.Controllers;

[ApiController]
[Route("api/[controller]")]
public class UserShipAssignmentsController : ControllerBase
{
    private readonly IUserShipAssignmentService _userShipAssignmentService;

    public UserShipAssignmentsController(IUserShipAssignmentService userShipAssignmentService)
    {
        _userShipAssignmentService = userShipAssignmentService;
    }

    [HttpPost("assign")]
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

    [HttpPost("unassign")]
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

    [HttpGet]
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

    [HttpGet("user/{userId}")]
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

    [HttpGet("ship/{shipCode}")]
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
