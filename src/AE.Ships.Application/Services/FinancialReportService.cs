using AE.Ships.Domain.DTOs;
using AE.Ships.Domain.Interfaces;

namespace AE.Ships.Application.Services;

public class FinancialReportService : IFinancialReportService
{
    private readonly IFinancialReportRepository _financialReportRepository;

    public FinancialReportService(IFinancialReportRepository financialReportRepository)
    {
        _financialReportRepository = financialReportRepository;
    }

    public async Task<IEnumerable<FinancialReportDto>> GetFinancialReportAsync(FinancialReportRequestDto request)
    {
        return await _financialReportRepository.GetFinancialReportAsync(request.ShipCode, request.AccountPeriod);
    }
}
