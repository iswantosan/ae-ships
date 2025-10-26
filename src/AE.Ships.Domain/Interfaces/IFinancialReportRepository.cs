using AE.Ships.Domain.DTOs;

namespace AE.Ships.Domain.Interfaces;

public interface IFinancialReportRepository
{
    Task<IEnumerable<FinancialReportDto>> GetFinancialReportAsync(string shipCode, DateTime accountPeriod);
}
