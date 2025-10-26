using AE.Ships.Application.Services;
using AE.Ships.Domain.DTOs;
using AE.Ships.Domain.Interfaces;
using Moq;
using Xunit;

namespace AE.Ships.Application.Tests.Services;

public class CrewServiceTests
{
    private readonly Mock<ICrewRepository> _mockRepository;
    private readonly CrewService _crewService;

    public CrewServiceTests()
    {
        _mockRepository = new Mock<ICrewRepository>();
        _crewService = new CrewService(_mockRepository.Object);
    }

    [Fact]
    public async Task GetCrewListForShipAsync_ShouldReturnCrewList()
    {
        var shipCode = "SHIP01";
        var request = new CrewListRequestDto
        {
            PageNumber = 1,
            PageSize = 10,
            SortColumn = "RankName",
            SortDirection = "ASC",
            SearchTerm = null
        };

        var crewList = new List<CrewListDto>
        {
            new CrewListDto
            {
                RankName = "Master",
                CrewMemberId = "CREW001",
                FirstName = "John",
                LastName = "Doe",
                Age = 45,
                Nationality = "Greek",
                SignOnDate = DateTime.Parse("2025-04-05"),
                Status = "Onboard"
            },
            new CrewListDto
            {
                RankName = "Chief Engineer",
                CrewMemberId = "CREW002",
                FirstName = "Jane",
                LastName = "Smith",
                Age = 40,
                Nationality = "Mexican",
                SignOnDate = DateTime.Parse("2025-04-04"),
                Status = "Onboard"
            }
        };

        _mockRepository.Setup(x => x.GetCrewListAsync(It.IsAny<CrewListRequestDto>()))
            .ReturnsAsync(crewList);

        var result = await _crewService.GetCrewListForShipAsync(shipCode, request);

        Assert.NotNull(result);
        Assert.Equal(2, result.Count());
        
        var crewArray = result.ToArray();
        Assert.Equal("Master", crewArray[0].RankName);
        Assert.Equal("CREW001", crewArray[0].CrewMemberId);
        Assert.Equal("John", crewArray[0].FirstName);
        Assert.Equal("Doe", crewArray[0].LastName);
        Assert.Equal(45, crewArray[0].Age);
        Assert.Equal("Greek", crewArray[0].Nationality);
        Assert.Equal("Onboard", crewArray[0].Status);

        _mockRepository.Verify(x => x.GetCrewListAsync(It.IsAny<CrewListRequestDto>()), Times.Once);
    }

    [Fact]
    public async Task GetCrewListForShipAsync_WhenNoCrew_ShouldReturnEmptyList()
    {
        var shipCode = "SHIP01";
        var request = new CrewListRequestDto
        {
            PageNumber = 1,
            PageSize = 10,
            SortColumn = "RankName",
            SortDirection = "ASC",
            SearchTerm = null
        };

        _mockRepository.Setup(x => x.GetCrewListAsync(It.IsAny<CrewListRequestDto>()))
            .ReturnsAsync(new List<CrewListDto>());

        var result = await _crewService.GetCrewListForShipAsync(shipCode, request);

        Assert.NotNull(result);
        Assert.Empty(result);
    }

    [Fact]
    public async Task GetCrewListForShipAsync_WithSearchTerm_ShouldPassSearchTermToRepository()
    {
        var shipCode = "SHIP01";
        var request = new CrewListRequestDto
        {
            PageNumber = 1,
            PageSize = 10,
            SortColumn = "FirstName",
            SortDirection = "DESC",
            SearchTerm = "John"
        };

        _mockRepository.Setup(x => x.GetCrewListAsync(It.IsAny<CrewListRequestDto>()))
            .ReturnsAsync(new List<CrewListDto>());

        var result = await _crewService.GetCrewListForShipAsync(shipCode, request);

        _mockRepository.Verify(x => x.GetCrewListAsync(It.IsAny<CrewListRequestDto>()), Times.Once);
    }

    [Fact]
    public async Task GetCrewMemberHistoryAsync_ShouldReturnHistory()
    {
        var crewMemberId = "CREW001";
        var history = new List<CrewMemberHistoryDto>
        {
            new CrewMemberHistoryDto
            {
                ServiceHistoryId = 1,
                CrewMemberId = "CREW001",
                ShipCode = "SHIP01",
                ShipName = "Flying Dutchman",
                RankName = "Master",
                SignOnDate = DateTime.Parse("2025-04-05"),
                SignOffDate = null,
                EndOfContractDate = DateTime.Parse("2025-07-05")
            },
            new CrewMemberHistoryDto
            {
                ServiceHistoryId = 2,
                CrewMemberId = "CREW001",
                ShipCode = "SHIP02",
                ShipName = "Thousand Sunny",
                RankName = "Chief Officer",
                SignOnDate = DateTime.Parse("2024-01-01"),
                SignOffDate = DateTime.Parse("2024-12-31"),
                EndOfContractDate = DateTime.Parse("2024-12-31")
            }
        };

        _mockRepository.Setup(x => x.GetCrewMemberHistoryAsync(crewMemberId)).ReturnsAsync(history);

        var result = await _crewService.GetCrewMemberHistoryAsync(crewMemberId);

        Assert.NotNull(result);
        Assert.Equal(2, result.Count());
        
        var historyArray = result.ToArray();
        Assert.Equal(1, historyArray[0].ServiceHistoryId);
        Assert.Equal("CREW001", historyArray[0].CrewMemberId);
        Assert.Equal("SHIP01", historyArray[0].ShipCode);
        Assert.Equal("Flying Dutchman", historyArray[0].ShipName);
        Assert.Equal("Master", historyArray[0].RankName);
        Assert.Null(historyArray[0].SignOffDate);

        _mockRepository.Verify(x => x.GetCrewMemberHistoryAsync(crewMemberId), Times.Once);
    }

    [Fact]
    public async Task GetCrewMemberHistoryAsync_WhenNoHistory_ShouldReturnEmptyList()
    {
        var crewMemberId = "INVALID";

        _mockRepository.Setup(x => x.GetCrewMemberHistoryAsync(crewMemberId)).ReturnsAsync(new List<CrewMemberHistoryDto>());

        var result = await _crewService.GetCrewMemberHistoryAsync(crewMemberId);

        Assert.NotNull(result);
        Assert.Empty(result);
    }

    [Fact]
    public async Task GetCrewListForShipAsync_ShouldPassAllParametersCorrectly()
    {
        var shipCode = "SHIP01";
        var request = new CrewListRequestDto
        {
            PageNumber = 2,
            PageSize = 5,
            SortColumn = "Age",
            SortDirection = "DESC",
            SearchTerm = "Greek"
        };

        _mockRepository.Setup(x => x.GetCrewListAsync(It.IsAny<CrewListRequestDto>()))
            .ReturnsAsync(new List<CrewListDto>());

        var result = await _crewService.GetCrewListForShipAsync(shipCode, request);

        _mockRepository.Verify(x => x.GetCrewListAsync(It.IsAny<CrewListRequestDto>()), Times.Once);
    }

    [Fact]
    public async Task GetCrewMemberHistoryAsync_WithEmptyCrewMemberId_ShouldPassEmptyString()
    {
        var crewMemberId = "";

        _mockRepository.Setup(x => x.GetCrewMemberHistoryAsync(crewMemberId)).ReturnsAsync(new List<CrewMemberHistoryDto>());

        var result = await _crewService.GetCrewMemberHistoryAsync(crewMemberId);

        _mockRepository.Verify(x => x.GetCrewMemberHistoryAsync(""), Times.Once);
    }
}
