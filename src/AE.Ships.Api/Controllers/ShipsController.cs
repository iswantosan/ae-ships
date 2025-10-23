using AE.Ships.Domain.DTOs;
using AE.Ships.Domain.Interfaces;
using Microsoft.AspNetCore.Mvc;

namespace AE.Ships.Api.Controllers;

[ApiController]
[Route("api/[controller]")]
public class ShipsController : ControllerBase
{
    private readonly IShipService _shipService;

    public ShipsController(IShipService shipService)
    {
        _shipService = shipService;
    }

    [HttpGet]
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

    [HttpGet("{code}")]
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

    [HttpPost]
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

    [HttpPut("{code}")]
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

    [HttpDelete("{code}")]
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

    [HttpGet("status/{status}")]
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

    [HttpGet("user/{userId}")]
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


