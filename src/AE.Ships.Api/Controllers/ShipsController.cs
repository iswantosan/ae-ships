using AE.Ships.Domain.DTOs;
using AE.Ships.Domain.Interfaces;
using Microsoft.AspNetCore.Mvc;
using Swashbuckle.AspNetCore.Annotations;
using Microsoft.AspNetCore.Authorization;

namespace AE.Ships.Api.Controllers;

/// <summary>
/// Ships management controller
/// </summary>
[ApiController]
[Route("api/[controller]")]
[SwaggerTag("Ships management endpoints for ship operations")]
[Authorize]
public class ShipsController : ControllerBase
{
    private readonly IShipService _shipService;

    public ShipsController(IShipService shipService)
    {
        _shipService = shipService;
    }

    /// <summary>
    /// Get all ships
    /// </summary>
    /// <returns>List of all ships</returns>
    /// <response code="200">Ships retrieved successfully</response>
    /// <response code="500">Internal server error</response>
    [HttpGet]
    [SwaggerOperation(
        Summary = "Get All Ships",
        Description = "Retrieves a list of all ships in the system",
        OperationId = "GetAllShips"
    )]
    [SwaggerResponse(200, "Ships retrieved successfully", typeof(IEnumerable<ShipDto>))]
    [SwaggerResponse(500, "Internal server error")]
    public async Task<ActionResult<IEnumerable<ShipDto>>> GetAllShips()
    {
        try
        {
            var ships = await _shipService.GetAllShipsAsync();
            return Ok(ships);
        }
        catch (Exception ex)
        {
            return StatusCode(500, new { message = "An error occurred while retrieving ships.", error = ex.Message });
        }
    }

    /// <summary>
    /// Get ship by code
    /// </summary>
    /// <param name="code">Ship code</param>
    /// <returns>Ship details</returns>
    /// <response code="200">Ship found</response>
    /// <response code="404">Ship not found</response>
    /// <response code="500">Internal server error</response>
    [HttpGet("{code}")]
    [SwaggerOperation(
        Summary = "Get Ship by Code",
        Description = "Retrieves a specific ship by its unique code",
        OperationId = "GetShip"
    )]
    [SwaggerResponse(200, "Ship found", typeof(ShipDto))]
    [SwaggerResponse(404, "Ship not found")]
    [SwaggerResponse(500, "Internal server error")]
    public async Task<ActionResult<ShipDto>> GetShip(string code)
    {
        try
        {
            var ship = await _shipService.GetShipByCodeAsync(code);
            if (ship == null)
            {
                return NotFound(new { message = $"Ship with code {code} not found." });
            }
            return Ok(ship);
        }
        catch (Exception ex)
        {
            return StatusCode(500, new { message = "An error occurred while retrieving the ship.", error = ex.Message });
        }
    }

    /// <summary>
    /// Create a new ship
    /// </summary>
    /// <param name="createShipDto">Ship creation data</param>
    /// <returns>Created ship details</returns>
    /// <response code="201">Ship created successfully</response>
    /// <response code="400">Invalid request data</response>
    /// <response code="500">Internal server error</response>
    [HttpPost]
    [SwaggerOperation(
        Summary = "Create Ship",
        Description = "Creates a new ship with the provided details",
        OperationId = "CreateShip"
    )]
    [SwaggerResponse(201, "Ship created successfully", typeof(ShipDto))]
    [SwaggerResponse(400, "Invalid request data")]
    [SwaggerResponse(500, "Internal server error")]
    public async Task<ActionResult<ShipDto>> CreateShip([FromBody] CreateShipDto createShipDto)
    {
        try
        {
            if (string.IsNullOrWhiteSpace(createShipDto.Code))
            {
                return BadRequest(new { message = "Ship code is required." });
            }

            if (string.IsNullOrWhiteSpace(createShipDto.Name))
            {
                return BadRequest(new { message = "Ship name is required." });
            }

            if (string.IsNullOrWhiteSpace(createShipDto.FiscalYear))
            {
                return BadRequest(new { message = "Fiscal year is required." });
            }

            if (string.IsNullOrWhiteSpace(createShipDto.Status))
            {
                return BadRequest(new { message = "Ship status is required." });
            }

            if (createShipDto.Status != "Active" && createShipDto.Status != "Inactive")
            {
                return BadRequest(new { message = "Ship status must be 'Active' or 'Inactive'." });
            }

            var ship = await _shipService.CreateShipAsync(createShipDto);
            return CreatedAtAction(nameof(GetShip), new { code = ship.Code }, ship);
        }
        catch (Exception ex)
        {
            return StatusCode(500, new { message = "An error occurred while creating the ship.", error = ex.Message });
        }
    }

    /// <summary>
    /// Update an existing ship
    /// </summary>
    /// <param name="code">Ship code</param>
    /// <param name="updateShipDto">Updated ship data</param>
    /// <returns>Updated ship details</returns>
    /// <response code="200">Ship updated successfully</response>
    /// <response code="400">Invalid request data</response>
    /// <response code="500">Internal server error</response>
    [HttpPut("{code}")]
    [SwaggerOperation(
        Summary = "Update Ship",
        Description = "Updates an existing ship with new details",
        OperationId = "UpdateShip"
    )]
    [SwaggerResponse(200, "Ship updated successfully", typeof(ShipDto))]
    [SwaggerResponse(400, "Invalid request data")]
    [SwaggerResponse(500, "Internal server error")]
    public async Task<ActionResult<ShipDto>> UpdateShip(string code, [FromBody] UpdateShipDto updateShipDto)
    {
        try
        {
            if (code != updateShipDto.Code)
            {
                return BadRequest(new { message = "Ship code mismatch." });
            }

            if (string.IsNullOrWhiteSpace(updateShipDto.Name))
            {
                return BadRequest(new { message = "Ship name is required." });
            }

            if (string.IsNullOrWhiteSpace(updateShipDto.FiscalYear))
            {
                return BadRequest(new { message = "Fiscal year is required." });
            }

            if (string.IsNullOrWhiteSpace(updateShipDto.Status))
            {
                return BadRequest(new { message = "Ship status is required." });
            }

            if (updateShipDto.Status != "Active" && updateShipDto.Status != "Inactive")
            {
                return BadRequest(new { message = "Ship status must be 'Active' or 'Inactive'." });
            }

            var ship = await _shipService.UpdateShipAsync(updateShipDto);
            return Ok(ship);
        }
        catch (Exception ex)
        {
            return StatusCode(500, new { message = "An error occurred while updating the ship.", error = ex.Message });
        }
    }

    /// <summary>
    /// Delete a ship
    /// </summary>
    /// <param name="code">Ship code</param>
    /// <returns>No content</returns>
    /// <response code="204">Ship deleted successfully</response>
    /// <response code="404">Ship not found</response>
    /// <response code="500">Internal server error</response>
    [HttpDelete("{code}")]
    [SwaggerOperation(
        Summary = "Delete Ship",
        Description = "Deletes a ship by its code",
        OperationId = "DeleteShip"
    )]
    [SwaggerResponse(204, "Ship deleted successfully")]
    [SwaggerResponse(404, "Ship not found")]
    [SwaggerResponse(500, "Internal server error")]
    public async Task<ActionResult> DeleteShip(string code)
    {
        try
        {
            var deleted = await _shipService.DeleteShipAsync(code);
            if (!deleted)
            {
                return NotFound(new { message = $"Ship with code {code} not found." });
            }
            return NoContent();
        }
        catch (Exception ex)
        {
            return StatusCode(500, new { message = "An error occurred while deleting the ship.", error = ex.Message });
        }
    }

    /// <summary>
    /// Get ships by status
    /// </summary>
    /// <param name="status">Ship status (Active or Inactive)</param>
    /// <returns>List of ships with specified status</returns>
    /// <response code="200">Ships retrieved successfully</response>
    /// <response code="400">Invalid status</response>
    /// <response code="500">Internal server error</response>
    [HttpGet("status/{status}")]
    [SwaggerOperation(
        Summary = "Get Ships by Status",
        Description = "Retrieves all ships with the specified status",
        OperationId = "GetShipsByStatus"
    )]
    [SwaggerResponse(200, "Ships retrieved successfully", typeof(IEnumerable<ShipDto>))]
    [SwaggerResponse(400, "Invalid status")]
    [SwaggerResponse(500, "Internal server error")]
    public async Task<ActionResult<IEnumerable<ShipDto>>> GetShipsByStatus(string status)
    {
        try
        {
            if (status != "Active" && status != "Inactive")
            {
                return BadRequest(new { message = "Status must be 'Active' or 'Inactive'." });
            }

            var ships = await _shipService.GetShipsByStatusAsync(status);
            return Ok(ships);
        }
        catch (Exception ex)
        {
            return StatusCode(500, new { message = "An error occurred while retrieving ships by status.", error = ex.Message });
        }
    }

    /// <summary>
    /// Get ships assigned to a user
    /// </summary>
    /// <param name="userId">User ID</param>
    /// <returns>List of ships assigned to the user</returns>
    /// <response code="200">Ships retrieved successfully</response>
    /// <response code="500">Internal server error</response>
    [HttpGet("user/{userId}")]
    [SwaggerOperation(
        Summary = "Get Ships by User",
        Description = "Retrieves all ships assigned to a specific user",
        OperationId = "GetShipsByUser"
    )]
    [SwaggerResponse(200, "Ships retrieved successfully", typeof(IEnumerable<ShipDto>))]
    [SwaggerResponse(500, "Internal server error")]
    public async Task<ActionResult<IEnumerable<ShipDto>>> GetShipsByUser(int userId)
    {
        try
        {
            var ships = await _shipService.GetShipsByUserAsync(userId);
            return Ok(ships);
        }
        catch (Exception ex)
        {
            return StatusCode(500, new { message = "An error occurred while retrieving ships by user.", error = ex.Message });
        }
    }
}


