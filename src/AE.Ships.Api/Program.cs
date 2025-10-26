using AE.Ships.Application.Services;
using AE.Ships.Domain.Interfaces;
using AE.Ships.Infrastructure.Data;

var builder = WebApplication.CreateBuilder(args);

builder.Services.AddControllers();
builder.Services.AddEndpointsApiExplorer();
builder.Services.AddSwaggerGen();

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
app.UseAuthorization();
app.MapControllers();

app.Run();