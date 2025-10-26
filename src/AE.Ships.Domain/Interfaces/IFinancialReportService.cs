using AE.Ships.Domain.DTOs;

namespace AE.Ships.Domain.Interfaces;

public interface IFinancialReportService
{
    Task<IEnumerable<FinancialReportDto>> GetFinancialReportAsync(FinancialReportRequestDto request);
}
