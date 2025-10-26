namespace AE.Ships.Domain.DTOs;

public class FinancialReportRequestDto
{
    public string ShipCode { get; set; } = string.Empty;
    public DateTime AccountPeriod { get; set; }
}
