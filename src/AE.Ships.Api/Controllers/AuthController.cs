using AE.Ships.Api.Services;
using Microsoft.AspNetCore.Mvc;
using Swashbuckle.AspNetCore.Annotations;
using Microsoft.AspNetCore.Authorization;

namespace AE.Ships.Api.Controllers;

/// <summary>
/// Authentication controller for JWT token management
/// </summary>
[ApiController]
[Route("api/[controller]")]
[SwaggerTag("Authentication endpoints for JWT token generation and validation")]
[AllowAnonymous]
public class AuthController : ControllerBase
{
    private readonly IJwtService _jwtService;

    public AuthController(IJwtService jwtService)
    {
        _jwtService = jwtService;
    }

    /// <summary>
    /// Authenticate user and generate JWT token
    /// </summary>
    /// <param name="request">Login credentials</param>
    /// <returns>JWT token and user information</returns>
    /// <response code="200">Login successful</response>
    /// <response code="400">Invalid request data</response>
    /// <response code="401">Invalid credentials</response>
    [HttpPost("login")]
    [SwaggerOperation(
        Summary = "User Login",
        Description = "Authenticates user credentials and returns JWT token",
        OperationId = "Login"
    )]
    [SwaggerResponse(200, "Login successful", typeof(LoginResponse))]
    [SwaggerResponse(400, "Invalid request data")]
    [SwaggerResponse(401, "Invalid credentials")]
    public IActionResult Login([FromBody] LoginRequest request)
    {
        if (string.IsNullOrWhiteSpace(request.Username) || string.IsNullOrWhiteSpace(request.Password))
        {
            return BadRequest(new { message = "Username and password are required." });
        }

        if (IsValidUser(request.Username, request.Password))
        {
            var role = GetUserRole(request.Username);
            var token = _jwtService.GenerateToken(request.Username, role);
            
            return Ok(new
            {
                token = token,
                username = request.Username,
                role = role,
                expires = DateTime.UtcNow.AddHours(24)
            });
        }

        return Unauthorized(new { message = "Invalid username or password." });
    }

    /// <summary>
    /// Validate JWT token
    /// </summary>
    /// <param name="request">Token validation request</param>
    /// <returns>Token validation result</returns>
    /// <response code="200">Token is valid</response>
    /// <response code="400">Invalid request data</response>
    /// <response code="401">Token is invalid</response>
    [HttpPost("validate")]
    [SwaggerOperation(
        Summary = "Validate Token",
        Description = "Validates JWT token and returns validation status",
        OperationId = "ValidateToken"
    )]
    [SwaggerResponse(200, "Token is valid", typeof(ValidateTokenResponse))]
    [SwaggerResponse(400, "Invalid request data")]
    [SwaggerResponse(401, "Token is invalid")]
    public IActionResult ValidateToken([FromBody] ValidateTokenRequest request)
    {
        if (string.IsNullOrWhiteSpace(request.Token))
        {
            return BadRequest(new { message = "Token is required." });
        }

        var isValid = _jwtService.ValidateToken(request.Token);
        
        if (isValid)
        {
            return Ok(new { valid = true, message = "Token is valid." });
        }

        return Unauthorized(new { valid = false, message = "Token is invalid." });
    }

    private static bool IsValidUser(string username, string password)
    {
        var validUsers = new Dictionary<string, string>
        {
            { "admin", "admin123" },
            { "user", "user123" },
            { "manager", "manager123" }
        };

        return validUsers.ContainsKey(username.ToLower()) && 
               validUsers[username.ToLower()] == password;
    }

    private static string GetUserRole(string username)
    {
        return username.ToLower() switch
        {
            "admin" => "Admin",
            "manager" => "Manager",
            _ => "User"
        };
    }
}

/// <summary>
/// Login request model
/// </summary>
public class LoginRequest
{
    /// <summary>
    /// Username for authentication
    /// </summary>
    /// <example>admin</example>
    public string Username { get; set; } = string.Empty;
    
    /// <summary>
    /// Password for authentication
    /// </summary>
    /// <example>admin123</example>
    public string Password { get; set; } = string.Empty;
}

/// <summary>
/// Token validation request model
/// </summary>
public class ValidateTokenRequest
{
    /// <summary>
    /// JWT token to validate
    /// </summary>
    public string Token { get; set; } = string.Empty;
}

/// <summary>
/// Login response model
/// </summary>
public class LoginResponse
{
    /// <summary>
    /// JWT token
    /// </summary>
    public string Token { get; set; } = string.Empty;
    
    /// <summary>
    /// Username
    /// </summary>
    public string Username { get; set; } = string.Empty;
    
    /// <summary>
    /// User role
    /// </summary>
    public string Role { get; set; } = string.Empty;
    
    /// <summary>
    /// Token expiration time
    /// </summary>
    public DateTime Expires { get; set; }
}

/// <summary>
/// Token validation response model
/// </summary>
public class ValidateTokenResponse
{
    /// <summary>
    /// Whether the token is valid
    /// </summary>
    public bool Valid { get; set; }
    
    /// <summary>
    /// Validation message
    /// </summary>
    public string Message { get; set; } = string.Empty;
}

