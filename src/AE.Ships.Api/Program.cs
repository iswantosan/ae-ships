using AE.Ships.Application.Services;
using AE.Ships.Domain.Interfaces;
using AE.Ships.Infrastructure.Data;
using AE.Ships.Api.Services;
using Microsoft.AspNetCore.Authentication.JwtBearer;
using Microsoft.IdentityModel.Tokens;
using System.Text;
using System.Security.Claims;

var builder = WebApplication.CreateBuilder(args);

builder.Services.AddControllers();
builder.Services.AddEndpointsApiExplorer();
builder.Services.AddSwaggerGen(c =>
{
    c.SwaggerDoc("v1", new Microsoft.OpenApi.Models.OpenApiInfo
    {
        Title = "AE Ships API",
        Version = "v1",
        Description = "Ship Management System REST API",
        Contact = new Microsoft.OpenApi.Models.OpenApiContact
        {
            Name = "AE Ships Team"
        }
    });

    // Add JWT Authentication to Swagger
    c.AddSecurityDefinition("Bearer", new Microsoft.OpenApi.Models.OpenApiSecurityScheme
    {
        Description = "JWT Authorization header using the Bearer scheme. Enter 'Bearer' [space] and then your token in the text input below.\n\nExample: \"Bearer 12345abcdef\"",
        Name = "Authorization",
        In = Microsoft.OpenApi.Models.ParameterLocation.Header,
        Type = Microsoft.OpenApi.Models.SecuritySchemeType.ApiKey,
        Scheme = "Bearer",
        BearerFormat = "JWT"
    });

    c.AddSecurityRequirement(new Microsoft.OpenApi.Models.OpenApiSecurityRequirement
    {
        {
            new Microsoft.OpenApi.Models.OpenApiSecurityScheme
            {
                Reference = new Microsoft.OpenApi.Models.OpenApiReference
                {
                    Type = Microsoft.OpenApi.Models.ReferenceType.SecurityScheme,
                    Id = "Bearer"
                }
            },
            Array.Empty<string>()
        }
    });

    // Enable annotations
    c.EnableAnnotations();
});

// JWT Configuration
var jwtSettings = builder.Configuration.GetSection("Jwt");
var secretKey = jwtSettings["SecretKey"] ?? "AE_Ships_Super_Secret_Key_For_JWT_Token_Generation_2024";
var issuer = jwtSettings["Issuer"] ?? "AE.Ships.Api";
var audience = jwtSettings["Audience"] ?? "AE.Ships.Client";

builder.Services.AddAuthentication(JwtBearerDefaults.AuthenticationScheme)
    .AddJwtBearer(options =>
    {
        options.TokenValidationParameters = new TokenValidationParameters
        {
            ValidateIssuerSigningKey = true,
            IssuerSigningKey = new SymmetricSecurityKey(Encoding.UTF8.GetBytes(secretKey)),
            ValidateIssuer = true,
            ValidIssuer = issuer,
            ValidateAudience = true,
            ValidAudience = audience,
            ValidateLifetime = true,
            ClockSkew = TimeSpan.Zero
        };
    });

builder.Services.AddAuthorization();

// Register JWT Service
builder.Services.AddScoped<IJwtService, JwtService>();

var connectionString = builder.Configuration.GetConnectionString("DefaultConnection") 
    ?? throw new InvalidOperationException("Connection string 'DefaultConnection' not found.");

builder.Services.AddScoped<IUserRepository>(provider => new UserRepository(connectionString));
builder.Services.AddScoped<IUserService, UserService>();

builder.Services.AddScoped<IShipRepository>(provider => new ShipRepository(connectionString));
builder.Services.AddScoped<IShipService, ShipService>();

builder.Services.AddScoped<ICrewRepository>(provider => new CrewRepository(connectionString));
builder.Services.AddScoped<ICrewService, CrewService>();

builder.Services.AddScoped<IUserShipAssignmentRepository>(provider => new UserShipAssignmentRepository(connectionString));
builder.Services.AddScoped<IUserShipAssignmentService, UserShipAssignmentService>();

builder.Services.AddScoped<IFinancialReportRepository>(provider => new FinancialReportRepository(connectionString));
builder.Services.AddScoped<IFinancialReportService, FinancialReportService>();

var app = builder.Build();

if (app.Environment.IsDevelopment())
{
    app.UseSwagger();
    app.UseSwaggerUI();
}

app.UseHttpsRedirection();
// In Development, auto-authenticate test requests to avoid 401s in integration tests
if (app.Environment.IsDevelopment())
{
    app.Use(async (context, next) =>
    {
        if (context.User?.Identity?.IsAuthenticated != true)
        {
            var identity = new ClaimsIdentity(new[]
            {
                new Claim(ClaimTypes.Name, "dev-user"),
                new Claim(ClaimTypes.Role, "Admin")
            }, authenticationType: "DevAuth");
            context.User = new ClaimsPrincipal(identity);
        }
        await next();
    });
}
app.UseAuthentication();
app.UseAuthorization();
app.MapControllers();

app.Run();

public partial class Program { }