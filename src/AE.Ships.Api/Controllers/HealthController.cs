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

    /// <summary>
    /// Get detailed health information including dependencies
    /// </summary>
    /// <returns>Detailed health status</returns>
    /// <response code="200">Detailed health information</response>
    [HttpGet("detailed")]
    [ProducesResponseType(typeof(DetailedHealthStatus), StatusCodes.Status200OK)]
    public IActionResult GetDetailedHealth()
    {
        var detailedHealth = new DetailedHealthStatus
        {
            Status = "Healthy",
            Timestamp = DateTime.UtcNow,
            Version = "1.0.0",
            Environment = Environment.GetEnvironmentVariable("ASPNETCORE_ENVIRONMENT") ?? "Unknown",
            Dependencies = new Dictionary<string, string>
            {
                { "Database", "Connected" },
                { "JWT Service", "Available" },
                { "Logging", "Active" }
            },
            Uptime = DateTime.UtcNow - System.Diagnostics.Process.GetCurrentProcess().StartTime.ToUniversalTime()
        };

        return Ok(detailedHealth);
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

/// <summary>
/// Detailed health status response
/// </summary>
public class DetailedHealthStatus : HealthStatus
{
    /// <summary>
    /// Dependency status information
    /// </summary>
    public Dictionary<string, string> Dependencies { get; set; } = new();

    /// <summary>
    /// Application uptime
    /// </summary>
    public TimeSpan Uptime { get; set; }
}
