using System.Net;
using System.Net.Http.Json;
using AE.Ships.Domain.DTOs;
using Microsoft.AspNetCore.Mvc.Testing;
using Xunit;

namespace AE.Ships.Api.Tests;

public class ShipsControllerIntegrationTests : IClassFixture<WebApplicationFactory<Program>>
{
    private readonly HttpClient _client;

    public ShipsControllerIntegrationTests(WebApplicationFactory<Program> factory)
    {
        _client = factory.CreateClient();
    }

    [Fact]
    public async Task GetAllShips_ShouldReturnSuccessStatusCode()
    {
        var response = await _client.GetAsync("/api/ships");

        response.EnsureSuccessStatusCode();
        Assert.Equal(HttpStatusCode.OK, response.StatusCode);
    }

    [Fact]
    public async Task GetShipByCode_WithValidCode_ShouldReturnShip()
    {
        var response = await _client.GetAsync("/api/ships/SHIP01");

        response.EnsureSuccessStatusCode();
        Assert.Equal(HttpStatusCode.OK, response.StatusCode);

        var ship = await response.Content.ReadFromJsonAsync<ShipDto>();
        Assert.NotNull(ship);
    }

    [Fact]
    public async Task GetShipByCode_WithInvalidCode_ShouldReturnNotFound()
    {
        var response = await _client.GetAsync("/api/ships/INVALID");

        Assert.Equal(HttpStatusCode.NotFound, response.StatusCode);
    }

    [Fact]
    public async Task CreateShip_WithValidData_ShouldCreateShip()
    {
        var createShipDto = new CreateShipDto
        {
            Code = "TEST01",
            Name = "Test Ship",
            FiscalYear = "0112",
            Status = "Active"
        };

        var response = await _client.PostAsJsonAsync("/api/ships", createShipDto);

        response.EnsureSuccessStatusCode();
        Assert.Equal(HttpStatusCode.Created, response.StatusCode);

        var ship = await response.Content.ReadFromJsonAsync<ShipDto>();
        Assert.NotNull(ship);
        Assert.Equal("TEST01", ship.Code);
        Assert.Equal("Test Ship", ship.Name);
    }

    [Fact]
    public async Task CreateShip_WithEmptyCode_ShouldReturnBadRequest()
    {
        var createShipDto = new CreateShipDto
        {
            Code = "",
            Name = "Test Ship",
            FiscalYear = "0112",
            Status = "Active"
        };

        var response = await _client.PostAsJsonAsync("/api/ships", createShipDto);

        Assert.Equal(HttpStatusCode.BadRequest, response.StatusCode);
    }

    [Fact]
    public async Task GetShipsByStatus_WithValidStatus_ShouldReturnShips()
    {
        var response = await _client.GetAsync("/api/ships/status/Active");

        response.EnsureSuccessStatusCode();
        Assert.Equal(HttpStatusCode.OK, response.StatusCode);
    }

    [Fact]
    public async Task GetShipsByUser_WithValidUserId_ShouldReturnShips()
    {
        var response = await _client.GetAsync("/api/ships/user/1");

        response.EnsureSuccessStatusCode();
        Assert.Equal(HttpStatusCode.OK, response.StatusCode);
    }
}
