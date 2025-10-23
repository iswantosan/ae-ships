CREATE PROCEDURE sp_GetShipByCode
    @ShipCode VARCHAR(20)
AS
BEGIN
    SET NOCOUNT ON;
    
    SELECT 
        Code,
        Name,
        FiscalYear,
        Status
    FROM Ships
    WHERE Code = @ShipCode;
END;
GO


