CREATE PROCEDURE sp_GetAllShips
AS
BEGIN
    SET NOCOUNT ON;
    
    SELECT 
        Code,
        Name,
        FiscalYear,
        Status
    FROM Ships
    ORDER BY Code;
END;
GO


