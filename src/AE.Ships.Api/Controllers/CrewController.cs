using AE.Ships.Domain.DTOs;
using AE.Ships.Domain.Interfaces;
using Microsoft.AspNetCore.Mvc;
using Swashbuckle.AspNetCore.Annotations;
using Microsoft.AspNetCore.Authorization;

namespace AE.Ships.Api.Controllers;

/// <summary>
/// Crew management controller
/// </summary>
[ApiController]
[Route("api/[controller]")]
[SwaggerTag("Crew management endpoints for ship crew operations")]
[Authorize]
public class CrewController : ControllerBase
{
    private readonly ICrewService _crewService;

    public CrewController(ICrewService crewService)
    {
        _crewService = crewService;
    }

    /// <summary>
    /// Get crew list for a specific ship
    /// </summary>
    /// <param name="shipCode">Ship code</param>
    /// <param name="pageNumber">Page number (default: 1)</param>
    /// <param name="pageSize">Page size (default: 10, max: 100)</param>
    /// <param name="sortColumn">Column to sort by (default: RankOrder)</param>
    /// <param name="sortDirection">Sort direction ASC or DESC (default: ASC)</param>
    /// <param name="searchTerm">Search term for filtering crew members</param>
    /// <returns>Paginated crew list</returns>
    /// <response code="200">Crew list retrieved successfully</response>
    /// <response code="400">Invalid request parameters</response>
    /// <response code="500">Internal server error</response>
    [HttpGet("ship/{shipCode}")]
    [SwaggerOperation(
        Summary = "Get Crew List for Ship",
        Description = "Retrieves a paginated list of crew members for a specific ship with optional filtering and sorting",
        OperationId = "GetCrewList"
    )]
    [SwaggerResponse(200, "Crew list retrieved successfully", typeof(IEnumerable<CrewListDto>))]
    [SwaggerResponse(400, "Invalid request parameters")]
    [SwaggerResponse(500, "Internal server error")]
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

            var crewList = await _crewService.GetCrewListForShipAsync(shipCode, request);
            return Ok(crewList);
        }
        catch (Exception ex)
        {
            return StatusCode(500, new { message = "An error occurred while retrieving crew list.", error = ex.Message });
        }
    }

    /// <summary>
    /// Get crew member history
    /// </summary>
    /// <param name="crewMemberId">Crew member ID</param>
    /// <returns>Crew member history</returns>
    /// <response code="200">Crew member history retrieved successfully</response>
    /// <response code="400">Invalid crew member ID</response>
    /// <response code="500">Internal server error</response>
    [HttpGet("member/{crewMemberId}")]
    [SwaggerOperation(
        Summary = "Get Crew Member History",
        Description = "Retrieves the complete history of assignments for a specific crew member",
        OperationId = "GetCrewMemberHistory"
    )]
    [SwaggerResponse(200, "Crew member history retrieved successfully", typeof(IEnumerable<CrewMemberHistoryDto>))]
    [SwaggerResponse(400, "Invalid crew member ID")]
    [SwaggerResponse(500, "Internal server error")]
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
