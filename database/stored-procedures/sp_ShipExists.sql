CREATE PROCEDURE sp_ShipExists
    @ShipCode VARCHAR(20)
AS
BEGIN
    SET NOCOUNT ON;
    
    SELECT CASE 
        WHEN EXISTS (SELECT 1 FROM Ships WHERE Code = @ShipCode) 
        THEN 1 
        ELSE 0 
    END;
END;
GO

