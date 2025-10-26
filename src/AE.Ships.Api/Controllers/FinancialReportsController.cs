using AE.Ships.Domain.DTOs;
using AE.Ships.Domain.Interfaces;
using Microsoft.AspNetCore.Mvc;

namespace AE.Ships.Api.Controllers;

[ApiController]
[Route("api/[controller]")]
public class FinancialReportsController : ControllerBase
{
    private readonly IFinancialReportService _financialReportService;

    public FinancialReportsController(IFinancialReportService financialReportService)
    {
        _financialReportService = financialReportService;
    }

    [HttpGet("ship/{shipCode}")]
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

    [HttpPost("ship/{shipCode}")]
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
