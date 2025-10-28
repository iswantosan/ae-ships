using AE.Ships.Domain.DTOs;
using AE.Ships.Domain.Interfaces;
using Microsoft.AspNetCore.Mvc;
using Swashbuckle.AspNetCore.Annotations;

namespace AE.Ships.Api.Controllers;

/// <summary>
/// Financial reports management controller
/// </summary>
[ApiController]
[Route("api/[controller]")]
[SwaggerTag("Financial reports management endpoints for ship financial operations")]
public class FinancialReportsController : ControllerBase
{
    private readonly IFinancialReportService _financialReportService;

    public FinancialReportsController(IFinancialReportService financialReportService)
    {
        _financialReportService = financialReportService;
    }

    /// <summary>
    /// Get financial report for a ship
    /// </summary>
    /// <param name="shipCode">Ship code</param>
    /// <param name="accountPeriod">Account period (date)</param>
    /// <returns>Financial report data</returns>
    /// <response code="200">Financial report retrieved successfully</response>
    /// <response code="400">Invalid request parameters</response>
    /// <response code="500">Internal server error</response>
    [HttpGet("ship/{shipCode}")]
    [SwaggerOperation(
        Summary = "Get Financial Report",
        Description = "Retrieves financial report data for a specific ship and account period",
        OperationId = "GetFinancialReport"
    )]
    [SwaggerResponse(200, "Financial report retrieved successfully", typeof(IEnumerable<FinancialReportDto>))]
    [SwaggerResponse(400, "Invalid request parameters")]
    [SwaggerResponse(500, "Internal server error")]
    public async Task<ActionResult<IEnumerable<FinancialReportDto>>> GetFinancialReport(
        string shipCode,
        [FromQuery] DateTime accountPeriod)
    {
        try
        {
            if (string.IsNullOrWhiteSpace(shipCode))
            {
                return BadRequest(new { message = "ShipCode cannot be empty." });
            }

            if (accountPeriod == default)
            {
                return BadRequest(new { message = "AccountPeriod is required." });
            }

            var request = new FinancialReportRequestDto
            {
                ShipCode = shipCode,
                AccountPeriod = accountPeriod
            };

            var report = await _financialReportService.GetFinancialReportAsync(request);
            return Ok(report);
        }
        catch (Exception ex)
        {
            return StatusCode(500, new { message = "An error occurred while retrieving the financial report.", error = ex.Message });
        }
    }

    /// <summary>
    /// Get financial report for a ship with request body
    /// </summary>
    /// <param name="shipCode">Ship code</param>
    /// <param name="request">Financial report request data</param>
    /// <returns>Financial report data</returns>
    /// <response code="200">Financial report retrieved successfully</response>
    /// <response code="400">Invalid request data</response>
    /// <response code="500">Internal server error</response>
    [HttpPost("ship/{shipCode}")]
    [SwaggerOperation(
        Summary = "Get Financial Report with Body",
        Description = "Retrieves financial report data for a specific ship using request body",
        OperationId = "GetFinancialReportWithBody"
    )]
    [SwaggerResponse(200, "Financial report retrieved successfully", typeof(IEnumerable<FinancialReportDto>))]
    [SwaggerResponse(400, "Invalid request data")]
    [SwaggerResponse(500, "Internal server error")]
    public async Task<ActionResult<IEnumerable<FinancialReportDto>>> GetFinancialReportWithBody(
        string shipCode,
        [FromBody] FinancialReportRequestDto request)
    {
        try
        {
            if (string.IsNullOrWhiteSpace(shipCode))
            {
                return BadRequest(new { message = "ShipCode cannot be empty." });
            }

            if (string.IsNullOrWhiteSpace(request.ShipCode))
            {
                return BadRequest(new { message = "ShipCode in request body is required." });
            }

            if (shipCode != request.ShipCode)
            {
                return BadRequest(new { message = "ShipCode in URL must match ShipCode in request body." });
            }

            if (request.AccountPeriod == default)
            {
                return BadRequest(new { message = "AccountPeriod is required." });
            }

            var report = await _financialReportService.GetFinancialReportAsync(request);
            return Ok(report);
        }
        catch (Exception ex)
        {
            return StatusCode(500, new { message = "An error occurred while retrieving the financial report.", error = ex.Message });
        }
    }
}
