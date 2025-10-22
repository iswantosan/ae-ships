CREATE PROCEDURE sp_GetShipsByStatus
    @Status VARCHAR(20)
AS
BEGIN
    SET NOCOUNT ON;
    
    SELECT 
        Code,
        Name,
        FiscalYear,
        Status
    FROM Ships
    WHERE Status = @Status
    ORDER BY Code;
END;
GO

