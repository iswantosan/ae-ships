using System.Net;
using Microsoft.AspNetCore.Mvc.Testing;
using Xunit;

namespace AE.Ships.Api.Tests;

public class CrewControllerIntegrationTests : IClassFixture<WebApplicationFactory<Program>>
{
    private readonly HttpClient _client;

    public CrewControllerIntegrationTests(WebApplicationFactory<Program> factory)
    {
        _client = factory.CreateClient();
    }

    [Fact]
    public async Task GetCrewListForShip_WithValidShipCode_ShouldReturnSuccessStatusCode()
    {
        var response = await _client.GetAsync("/api/crew/ship/SHIP01");

        response.EnsureSuccessStatusCode();
        Assert.Equal(HttpStatusCode.OK, response.StatusCode);
    }

    [Fact]
    public async Task GetCrewListForShip_WithInvalidShipCode_ShouldReturnBadRequest()
    {
        var response = await _client.GetAsync("/api/crew/ship/");

        Assert.Equal(HttpStatusCode.BadRequest, response.StatusCode);
    }

    [Fact]
    public async Task GetCrewListForShip_WithPagination_ShouldReturnSuccessStatusCode()
    {
        var response = await _client.GetAsync("/api/crew/ship/SHIP01?pageNumber=1&pageSize=5");

        response.EnsureSuccessStatusCode();
        Assert.Equal(HttpStatusCode.OK, response.StatusCode);
    }

    [Fact]
    public async Task GetCrewListForShip_WithInvalidPagination_ShouldReturnBadRequest()
    {
        var response = await _client.GetAsync("/api/crew/ship/SHIP01?pageNumber=0&pageSize=5");

        Assert.Equal(HttpStatusCode.BadRequest, response.StatusCode);
    }

    [Fact]
    public async Task GetCrewMemberHistory_WithValidCrewMemberId_ShouldReturnSuccessStatusCode()
    {
        var response = await _client.GetAsync("/api/crew/member/CREW001");

        response.EnsureSuccessStatusCode();
        Assert.Equal(HttpStatusCode.OK, response.StatusCode);
    }

    [Fact]
    public async Task GetCrewMemberHistory_WithInvalidCrewMemberId_ShouldReturnBadRequest()
    {
        var response = await _client.GetAsync("/api/crew/member/");

        Assert.Equal(HttpStatusCode.BadRequest, response.StatusCode);
    }
}
