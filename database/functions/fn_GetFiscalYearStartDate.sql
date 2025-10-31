CREATE FUNCTION fn_GetFiscalYearStartDate(
    @FiscalYear CHAR(4),
    @PeriodDate DATE
)
RETURNS DATE
AS
BEGIN
    DECLARE @FiscalStartMonth INT;
    DECLARE @PeriodYear INT = YEAR(@PeriodDate);
    DECLARE @PeriodMonth INT = MONTH(@PeriodDate);
    DECLARE @FiscalYearStartDate DATE;
    
    SET @FiscalStartMonth = CAST(SUBSTRING(@FiscalYear, 1, 2) AS INT);
    
    IF @PeriodMonth >= @FiscalStartMonth
        SET @FiscalYearStartDate = DATEFROMPARTS(@PeriodYear, @FiscalStartMonth, 1);
    ELSE
        SET @FiscalYearStartDate = DATEFROMPARTS(@PeriodYear - 1, @FiscalStartMonth, 1);
    
    RETURN @FiscalYearStartDate;
END;
GO




