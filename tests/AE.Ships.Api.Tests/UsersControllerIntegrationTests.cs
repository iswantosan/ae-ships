using System.Net;
using System.Net.Http.Json;
using AE.Ships.Domain.DTOs;
using Microsoft.AspNetCore.Mvc.Testing;
using Xunit;

namespace AE.Ships.Api.Tests;

public class UsersControllerIntegrationTests : IClassFixture<WebApplicationFactory<Program>>
{
    private readonly HttpClient _client;

    public UsersControllerIntegrationTests(WebApplicationFactory<Program> factory)
    {
        _client = factory.CreateClient();
    }

    [Fact]
    public async Task GetAllUsers_ShouldReturnSuccessStatusCode()
    {
        var response = await _client.GetAsync("/api/users");

        response.EnsureSuccessStatusCode();
        Assert.Equal(HttpStatusCode.OK, response.StatusCode);
    }

    [Fact]
    public async Task GetUserById_WithValidId_ShouldReturnUser()
    {
        var response = await _client.GetAsync("/api/users/1");

        response.EnsureSuccessStatusCode();
        Assert.Equal(HttpStatusCode.OK, response.StatusCode);

        var user = await response.Content.ReadFromJsonAsync<UserDto>();
        Assert.NotNull(user);
    }

    [Fact]
    public async Task GetUserById_WithInvalidId_ShouldReturnNotFound()
    {
        var response = await _client.GetAsync("/api/users/99999");

        Assert.Equal(HttpStatusCode.NotFound, response.StatusCode);
    }

    [Fact]
    public async Task CreateUser_WithValidData_ShouldCreateUser()
    {
        var createUserDto = new CreateUserDto
        {
            Name = "Test User",
            Role = "Test Role"
        };

        var response = await _client.PostAsJsonAsync("/api/users", createUserDto);

        response.EnsureSuccessStatusCode();
        Assert.Equal(HttpStatusCode.Created, response.StatusCode);

        var user = await response.Content.ReadFromJsonAsync<UserDto>();
        Assert.NotNull(user);
        Assert.Equal("Test User", user.Name);
        Assert.Equal("Test Role", user.Role);
    }

    [Fact]
    public async Task CreateUser_WithEmptyName_ShouldReturnBadRequest()
    {
        var createUserDto = new CreateUserDto
        {
            Name = "",
            Role = "Test Role"
        };

        var response = await _client.PostAsJsonAsync("/api/users", createUserDto);

        Assert.Equal(HttpStatusCode.BadRequest, response.StatusCode);
    }
}
