CREATE PROCEDURE sp_DeleteShip
    @ShipCode VARCHAR(20)
AS
BEGIN
    SET NOCOUNT ON;
    
    DELETE FROM Ships
    WHERE Code = @ShipCode;
END;
GO


