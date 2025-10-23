CREATE PROCEDURE sp_UnassignShipFromUser
    @UserId INT,
    @ShipCode VARCHAR(20)
AS
BEGIN
    SET NOCOUNT ON;
    
    IF NOT EXISTS (SELECT 1 FROM UserShipAssignments WHERE UserId = @UserId AND ShipCode = @ShipCode)
    BEGIN
        RAISERROR('Ship %s is not assigned to user %d.', 16, 1, @ShipCode, @UserId);
        RETURN;
    END
    
    DELETE FROM UserShipAssignments 
    WHERE UserId = @UserId AND ShipCode = @ShipCode;
    
    SELECT @@ROWCOUNT AS RowsAffected;
END;
GO
