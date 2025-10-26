using AE.Ships.Application.Services;
using AE.Ships.Domain.DTOs;
using AE.Ships.Domain.Interfaces;
using Moq;
using Xunit;

namespace AE.Ships.Application.Tests.Services;

public class FinancialReportServiceTests
{
    private readonly Mock<IFinancialReportRepository> _mockRepository;
    private readonly FinancialReportService _financialReportService;

    public FinancialReportServiceTests()
    {
        _mockRepository = new Mock<IFinancialReportRepository>();
        _financialReportService = new FinancialReportService(_mockRepository.Object);
    }

    [Fact]
    public async Task GetFinancialReportAsync_ShouldReturnFinancialReport()
    {
        var request = new FinancialReportRequestDto
        {
            ShipCode = "SHIP01",
            AccountPeriod = DateTime.Parse("2025-07-01")
        };

        var report = new List<FinancialReportDto>
        {
            new FinancialReportDto
            {
                CoaDescription = "OPERATING EXPENSES",
                AccountNumber = "7000000",
                Actual = 3300m,
                Budget = 3300m,
                Variance = 0m,
                ActualYtd = 9600m,
                BudgetYtd = 9900m,
                VarianceYtd = -300m
            },
            new FinancialReportDto
            {
                CoaDescription = "AWARD AND GRANT TO INDIVIDUALS",
                AccountNumber = "7100000",
                Actual = 3300m,
                Budget = 3300m,
                Variance = 0m,
                ActualYtd = 9600m,
                BudgetYtd = 9900m,
                VarianceYtd = -300m
            },
            new FinancialReportDto
            {
                CoaDescription = "AWARDS",
                AccountNumber = "7120000",
                Actual = 1000m,
                Budget = 1100m,
                Variance = -100m,
                ActualYtd = 3200m,
                BudgetYtd = 3300m,
                VarianceYtd = -100m
            },
            new FinancialReportDto
            {
                CoaDescription = "SCHOLARSHIPS",
                AccountNumber = "7135000",
                Actual = 2300m,
                Budget = 2200m,
                Variance = 100m,
                ActualYtd = 6400m,
                BudgetYtd = 6300m,
                VarianceYtd = 100m
            }
        };

        _mockRepository.Setup(x => x.GetFinancialReportAsync(request.ShipCode, request.AccountPeriod))
            .ReturnsAsync(report);

        var result = await _financialReportService.GetFinancialReportAsync(request);

        Assert.NotNull(result);
        Assert.Equal(4, result.Count());
        
        var reportArray = result.ToArray();
        Assert.Equal("OPERATING EXPENSES", reportArray[0].CoaDescription);
        Assert.Equal("7000000", reportArray[0].AccountNumber);
        Assert.Equal(3300m, reportArray[0].Actual);
        Assert.Equal(3300m, reportArray[0].Budget);
        Assert.Equal(0m, reportArray[0].Variance);
        Assert.Equal(9600m, reportArray[0].ActualYtd);
        Assert.Equal(9900m, reportArray[0].BudgetYtd);
        Assert.Equal(-300m, reportArray[0].VarianceYtd);

        _mockRepository.Verify(x => x.GetFinancialReportAsync("SHIP01", DateTime.Parse("2025-07-01")), Times.Once);
    }

    [Fact]
    public async Task GetFinancialReportAsync_WhenNoData_ShouldReturnEmptyList()
    {
        var request = new FinancialReportRequestDto
        {
            ShipCode = "INVALID",
            AccountPeriod = DateTime.Parse("2025-07-01")
        };

        _mockRepository.Setup(x => x.GetFinancialReportAsync(request.ShipCode, request.AccountPeriod))
            .ReturnsAsync(new List<FinancialReportDto>());

        var result = await _financialReportService.GetFinancialReportAsync(request);

        Assert.NotNull(result);
        Assert.Empty(result);
    }

    [Fact]
    public async Task GetFinancialReportAsync_WithNullValues_ShouldHandleNullValues()
    {
        var request = new FinancialReportRequestDto
        {
            ShipCode = "SHIP01",
            AccountPeriod = DateTime.Parse("2025-01-01")
        };

        var report = new List<FinancialReportDto>
        {
            new FinancialReportDto
            {
                CoaDescription = "TEST ACCOUNT",
                AccountNumber = "8000000",
                Actual = null,
                Budget = 1000m,
                Variance = null,
                ActualYtd = null,
                BudgetYtd = 5000m,
                VarianceYtd = null
            }
        };

        _mockRepository.Setup(x => x.GetFinancialReportAsync(request.ShipCode, request.AccountPeriod))
            .ReturnsAsync(report);

        var result = await _financialReportService.GetFinancialReportAsync(request);

        Assert.NotNull(result);
        Assert.Single(result);
        
        var reportItem = result.First();
        Assert.Equal("TEST ACCOUNT", reportItem.CoaDescription);
        Assert.Equal("8000000", reportItem.AccountNumber);
        Assert.Null(reportItem.Actual);
        Assert.Equal(1000m, reportItem.Budget);
        Assert.Null(reportItem.Variance);
        Assert.Null(reportItem.ActualYtd);
        Assert.Equal(5000m, reportItem.BudgetYtd);
        Assert.Null(reportItem.VarianceYtd);
    }

    [Fact]
    public async Task GetFinancialReportAsync_WithZeroValues_ShouldHandleZeroValues()
    {
        var request = new FinancialReportRequestDto
        {
            ShipCode = "SHIP01",
            AccountPeriod = DateTime.Parse("2025-06-01")
        };

        var report = new List<FinancialReportDto>
        {
            new FinancialReportDto
            {
                CoaDescription = "ZERO ACCOUNT",
                AccountNumber = "9000000",
                Actual = 0m,
                Budget = 0m,
                Variance = 0m,
                ActualYtd = 0m,
                BudgetYtd = 0m,
                VarianceYtd = 0m
            }
        };

        _mockRepository.Setup(x => x.GetFinancialReportAsync(request.ShipCode, request.AccountPeriod))
            .ReturnsAsync(report);

        var result = await _financialReportService.GetFinancialReportAsync(request);

        Assert.NotNull(result);
        Assert.Single(result);
        
        var reportItem = result.First();
        Assert.Equal(0m, reportItem.Actual);
        Assert.Equal(0m, reportItem.Budget);
        Assert.Equal(0m, reportItem.Variance);
        Assert.Equal(0m, reportItem.ActualYtd);
        Assert.Equal(0m, reportItem.BudgetYtd);
        Assert.Equal(0m, reportItem.VarianceYtd);
    }

    [Fact]
    public async Task GetFinancialReportAsync_ShouldPassParametersCorrectly()
    {
        var request = new FinancialReportRequestDto
        {
            ShipCode = "TEST01",
            AccountPeriod = DateTime.Parse("2025-12-31")
        };

        _mockRepository.Setup(x => x.GetFinancialReportAsync(It.IsAny<string>(), It.IsAny<DateTime>()))
            .ReturnsAsync(new List<FinancialReportDto>());

        var result = await _financialReportService.GetFinancialReportAsync(request);

        _mockRepository.Verify(x => x.GetFinancialReportAsync("TEST01", DateTime.Parse("2025-12-31")), Times.Once);
    }

    [Fact]
    public async Task GetFinancialReportAsync_WithDifferentFiscalYears_ShouldPassCorrectDate()
    {
        var request = new FinancialReportRequestDto
        {
            ShipCode = "SHIP02",
            AccountPeriod = DateTime.Parse("2025-03-31")
        };

        _mockRepository.Setup(x => x.GetFinancialReportAsync(It.IsAny<string>(), It.IsAny<DateTime>()))
            .ReturnsAsync(new List<FinancialReportDto>());

        var result = await _financialReportService.GetFinancialReportAsync(request);

        _mockRepository.Verify(x => x.GetFinancialReportAsync("SHIP02", DateTime.Parse("2025-03-31")), Times.Once);
    }

    [Fact]
    public async Task GetFinancialReportAsync_WithMultipleAccounts_ShouldReturnAllAccounts()
    {
        var request = new FinancialReportRequestDto
        {
            ShipCode = "SHIP01",
            AccountPeriod = DateTime.Parse("2025-07-01")
        };

        var report = new List<FinancialReportDto>
        {
            new FinancialReportDto
            {
                CoaDescription = "PARENT ACCOUNT",
                AccountNumber = "7000000",
                Actual = 5000m,
                Budget = 5000m,
                Variance = 0m,
                ActualYtd = 15000m,
                BudgetYtd = 15000m,
                VarianceYtd = 0m
            },
            new FinancialReportDto
            {
                CoaDescription = "CHILD ACCOUNT 1",
                AccountNumber = "7100000",
                Actual = 2000m,
                Budget = 2000m,
                Variance = 0m,
                ActualYtd = 6000m,
                BudgetYtd = 6000m,
                VarianceYtd = 0m
            },
            new FinancialReportDto
            {
                CoaDescription = "CHILD ACCOUNT 2",
                AccountNumber = "7200000",
                Actual = 3000m,
                Budget = 3000m,
                Variance = 0m,
                ActualYtd = 9000m,
                BudgetYtd = 9000m,
                VarianceYtd = 0m
            }
        };

        _mockRepository.Setup(x => x.GetFinancialReportAsync(request.ShipCode, request.AccountPeriod))
            .ReturnsAsync(report);

        var result = await _financialReportService.GetFinancialReportAsync(request);

        Assert.NotNull(result);
        Assert.Equal(3, result.Count());
        
        var reportList = result.ToList();
        Assert.Equal("PARENT ACCOUNT", reportList[0].CoaDescription);
        Assert.Equal("CHILD ACCOUNT 1", reportList[1].CoaDescription);
        Assert.Equal("CHILD ACCOUNT 2", reportList[2].CoaDescription);
    }
}

