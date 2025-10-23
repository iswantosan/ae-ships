using AE.Ships.Domain.DTOs;
using AE.Ships.Domain.Interfaces;
using Microsoft.AspNetCore.Mvc;

namespace AE.Ships.Api.Controllers;

[ApiController]
[Route("api/[controller]")]
public class CrewController : ControllerBase
{
    private readonly ICrewService _crewService;

    public CrewController(ICrewService crewService)
    {
        _crewService = crewService;
    }

    [HttpGet("ship/{shipCode}")]
    public async Task<ActionResult<IEnumerable<CrewListDto>>> GetCrewList(
        string shipCode,
        [FromQuery] int pageNumber = 1,
        [FromQuery] int pageSize = 10,
        [FromQuery] string sortColumn = "RankOrder",
        [FromQuery] string sortDirection = "ASC",
        [FromQuery] string? searchTerm = null)
    {
        try
        {
            if (string.IsNullOrWhiteSpace(shipCode))
            {
                return BadRequest(new { message = "Ship code is required." });
            }

            if (pageNumber < 1)
            {
                return BadRequest(new { message = "Page number must be greater than 0." });
            }

            if (pageSize < 1 || pageSize > 100)
            {
                return BadRequest(new { message = "Page size must be between 1 and 100." });
            }

            if (sortDirection != "ASC" && sortDirection != "DESC")
            {
                return BadRequest(new { message = "Sort direction must be 'ASC' or 'DESC'." });
            }

            var request = new CrewListRequestDto
            {
                ShipCode = shipCode,
                PageNumber = pageNumber,
                PageSize = pageSize,
                SortColumn = sortColumn,
                SortDirection = sortDirection,
                SearchTerm = searchTerm
            };

            var crewList = await _crewService.GetCrewListAsync(request);
            return Ok(crewList);
        }
        catch (Exception ex)
        {
            return StatusCode(500, new { message = "An error occurred while retrieving crew list.", error = ex.Message });
        }
    }

    [HttpGet("member/{crewMemberId}")]
    public async Task<ActionResult<IEnumerable<CrewMemberHistoryDto>>> GetCrewMemberHistory(string crewMemberId)
    {
        try
        {
            if (string.IsNullOrWhiteSpace(crewMemberId))
            {
                return BadRequest(new { message = "Crew member ID is required." });
            }

            var history = await _crewService.GetCrewMemberHistoryAsync(crewMemberId);
            return Ok(history);
        }
        catch (Exception ex)
        {
            return StatusCode(500, new { message = "An error occurred while retrieving crew member history.", error = ex.Message });
        }
    }
}
