namespace AE.Ships.Domain.DTOs;

public class FinancialReportDto
{
    public string CoaDescription { get; set; } = string.Empty;
    public string AccountNumber { get; set; } = string.Empty;
    public decimal? Actual { get; set; }
    public decimal? Budget { get; set; }
    public decimal? Variance { get; set; }
    public decimal? ActualYtd { get; set; }
    public decimal? BudgetYtd { get; set; }
    public decimal? VarianceYtd { get; set; }
}
