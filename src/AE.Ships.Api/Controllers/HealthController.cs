using Microsoft.AspNetCore.Mvc;

namespace AE.Ships.Api.Controllers;

/// <summary>
/// Health check controller for monitoring API status
/// </summary>
[ApiController]
[Route("api/[controller]")]
public class HealthController : ControllerBase
{
    /// <summary>
    /// Get API health status
    /// </summary>
    /// <returns>Health status information</returns>
    /// <response code="200">API is healthy</response>
    [HttpGet]
    [ProducesResponseType(typeof(HealthStatus), StatusCodes.Status200OK)]
    public IActionResult GetHealth()
    {
        var healthStatus = new HealthStatus
        {
            Status = "Healthy",
            Timestamp = DateTime.UtcNow,
            Version = "1.0.0",
            Environment = Environment.GetEnvironmentVariable("ASPNETCORE_ENVIRONMENT") ?? "Unknown"
        };

        return Ok(healthStatus);
    }

}

/// <summary>
/// Basic health status response
/// </summary>
public class HealthStatus
{
    /// <summary>
    /// Current health status
    /// </summary>
    public string Status { get; set; } = string.Empty;

    /// <summary>
    /// Timestamp of the health check
    /// </summary>
    public DateTime Timestamp { get; set; }

    /// <summary>
    /// API version
    /// </summary>
    public string Version { get; set; } = string.Empty;

    /// <summary>
    /// Current environment
    /// </summary>
    public string Environment { get; set; } = string.Empty;
}

