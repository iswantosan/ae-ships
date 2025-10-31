using System.Diagnostics;

namespace AE.Ships.Api.Middleware;

/// <summary>
/// Middleware for logging HTTP requests and responses
/// </summary>
public class RequestLoggingMiddleware
{
    private readonly RequestDelegate _next;
    private readonly ILogger<RequestLoggingMiddleware> _logger;

    public RequestLoggingMiddleware(RequestDelegate next, ILogger<RequestLoggingMiddleware> logger)
    {
        _next = next;
        _logger = logger;
    }

    public async Task InvokeAsync(HttpContext context)
    {
        var correlationId = context.Request.Headers["X-Correlation-ID"].FirstOrDefault() 
            ?? Guid.NewGuid().ToString();
        
        // Add correlation ID to response headers
        context.Response.Headers["X-Correlation-ID"] = correlationId;
        
        var stopwatch = Stopwatch.StartNew();
        var path = context.Request.Path;
        var method = context.Request.Method;
        var queryString = context.Request.QueryString.HasValue ? context.Request.QueryString.ToString() : "";
        
        // Log incoming request
        _logger.LogInformation(
            "[{CorrelationId}] {Method} {Path}{QueryString} - Request started from {RemoteIp}",
            correlationId, method, path, queryString, context.Connection.RemoteIpAddress);

        try
        {
            await _next(context);
            
            stopwatch.Stop();
            var statusCode = context.Response.StatusCode;
            
            // Log response
            _logger.LogInformation(
                "[{CorrelationId}] {Method} {Path}{QueryString} - Response {StatusCode} returned in {ElapsedMilliseconds}ms",
                correlationId, method, path, queryString, statusCode, stopwatch.ElapsedMilliseconds);
            
            // Log warnings for 4xx and 5xx status codes
            if (statusCode >= 400 && statusCode < 500)
            {
                _logger.LogWarning(
                    "[{CorrelationId}] {Method} {Path} - Client error: {StatusCode}",
                    correlationId, method, path, statusCode);
            }
            else if (statusCode >= 500)
            {
                _logger.LogError(
                    "[{CorrelationId}] {Method} {Path} - Server error: {StatusCode}",
                    correlationId, method, path, statusCode);
            }
        }
        catch (Exception ex)
        {
            stopwatch.Stop();
            var statusCode = context.Response.StatusCode;
            
            _logger.LogError(ex,
                "[{CorrelationId}] {Method} {Path}{QueryString} - Unhandled exception after {ElapsedMilliseconds}ms: {Message}",
                correlationId, method, path, queryString, stopwatch.ElapsedMilliseconds, ex.Message);
            
            // Re-throw to let exception middleware handle it
            throw;
        }
    }
}

