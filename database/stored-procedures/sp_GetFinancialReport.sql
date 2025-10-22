CREATE PROCEDURE sp_GetFinancialReport
    @ShipCode VARCHAR(20),
    @AccountPeriod DATE
AS
BEGIN
    SET NOCOUNT ON;
    
    DECLARE @FiscalYear CHAR(4);
    DECLARE @FiscalStartMonth INT;
    DECLARE @PeriodYear INT = YEAR(@AccountPeriod);
    DECLARE @PeriodMonth INT = MONTH(@AccountPeriod);
    DECLARE @FiscalYearStartDate DATE;
    
    SELECT @FiscalYear = FiscalYear FROM Ships WHERE Code = @ShipCode;
    
    SET @FiscalStartMonth = CAST(SUBSTRING(@FiscalYear, 1, 2) AS INT);
    
    IF @PeriodMonth >= @FiscalStartMonth
        SET @FiscalYearStartDate = DATEFROMPARTS(@PeriodYear, @FiscalStartMonth, 1);
    ELSE
        SET @FiscalYearStartDate = DATEFROMPARTS(@PeriodYear - 1, @FiscalStartMonth, 1);
    
    WITH AccountHierarchy AS (
        SELECT 
            AccountNumber,
            Description,
            ParentAccountNumber,
            AccountType,
            CAST(AccountNumber AS VARCHAR(MAX)) AS Path,
            0 AS Level
        FROM ChartOfAccounts
        WHERE ParentAccountNumber IS NULL
        
        UNION ALL
        
        SELECT 
            c.AccountNumber,
            c.Description,
            c.ParentAccountNumber,
            c.AccountType,
            CAST(ah.Path + '|' + c.AccountNumber AS VARCHAR(MAX)),
            ah.Level + 1
        FROM ChartOfAccounts c
        INNER JOIN AccountHierarchy ah ON c.ParentAccountNumber = ah.AccountNumber
    ),
    LeafAccountData AS (
        SELECT 
            ah.AccountNumber,
            ah.Description,
            ah.ParentAccountNumber,
            ah.Path,
            ah.Level,
            ISNULL(SUM(CASE WHEN at.AccountPeriod = @AccountPeriod THEN at.ActualValue ELSE 0 END), 0) AS ActualPeriod,
            ISNULL(SUM(CASE WHEN bd.AccountPeriod = @AccountPeriod THEN bd.BudgetValue ELSE 0 END), 0) AS BudgetPeriod,
            ISNULL(SUM(CASE WHEN at.AccountPeriod >= @FiscalYearStartDate AND at.AccountPeriod <= @AccountPeriod THEN at.ActualValue ELSE 0 END), 0) AS ActualYTD,
            ISNULL(SUM(CASE WHEN bd.AccountPeriod >= @FiscalYearStartDate AND bd.AccountPeriod <= @AccountPeriod THEN bd.BudgetValue ELSE 0 END), 0) AS BudgetYTD
        FROM AccountHierarchy ah
        LEFT JOIN AccountTransactions at ON at.AccountNumber = ah.AccountNumber AND at.ShipCode = @ShipCode
        LEFT JOIN BudgetData bd ON bd.AccountNumber = ah.AccountNumber AND bd.ShipCode = @ShipCode
        WHERE ah.AccountType = 'Child'
        GROUP BY ah.AccountNumber, ah.Description, ah.ParentAccountNumber, ah.Path, ah.Level
    ),
    AggregatedData AS (
        SELECT 
            ah.AccountNumber,
            ah.Description,
            ah.ParentAccountNumber,
            ah.Path,
            ah.Level,
            SUM(lad.ActualPeriod) AS ActualPeriod,
            SUM(lad.BudgetPeriod) AS BudgetPeriod,
            SUM(lad.ActualYTD) AS ActualYTD,
            SUM(lad.BudgetYTD) AS BudgetYTD
        FROM AccountHierarchy ah
        INNER JOIN LeafAccountData lad ON lad.Path LIKE ah.AccountNumber + '%' OR lad.AccountNumber = ah.AccountNumber
        GROUP BY ah.AccountNumber, ah.Description, ah.ParentAccountNumber, ah.Path, ah.Level
    )
    SELECT 
        Description AS [COA Description],
        AccountNumber AS [Account Number],
        CASE WHEN ActualPeriod = 0 THEN NULL ELSE ActualPeriod END AS [Actual],
        CASE WHEN BudgetPeriod = 0 THEN NULL ELSE BudgetPeriod END AS [Budget],
        CASE WHEN ActualPeriod - BudgetPeriod = 0 THEN NULL ELSE ActualPeriod - BudgetPeriod END AS [Variance],
        CASE WHEN ActualYTD = 0 THEN NULL ELSE ActualYTD END AS [Actual YTD],
        CASE WHEN BudgetYTD = 0 THEN NULL ELSE BudgetYTD END AS [Budget YTD],
        CASE WHEN ActualYTD - BudgetYTD = 0 THEN NULL ELSE ActualYTD - BudgetYTD END AS [Variance YTD]
    FROM AggregatedData
    WHERE ActualPeriod <> 0 OR BudgetPeriod <> 0 OR ActualYTD <> 0 OR BudgetYTD <> 0
    ORDER BY Path;
END;
GO

