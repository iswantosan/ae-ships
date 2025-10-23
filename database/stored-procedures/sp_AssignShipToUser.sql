CREATE PROCEDURE sp_AssignShipToUser
    @UserId INT,
    @ShipCode VARCHAR(20)
AS
BEGIN
    SET NOCOUNT ON;
    
    IF NOT EXISTS (SELECT 1 FROM Users WHERE UserId = @UserId)
    BEGIN
        RAISERROR('User with ID %d does not exist.', 16, 1, @UserId);
        RETURN;
    END
    
    IF NOT EXISTS (SELECT 1 FROM Ships WHERE Code = @ShipCode)
    BEGIN
        RAISERROR('Ship with code %s does not exist.', 16, 1, @ShipCode);
        RETURN;
    END
    
    IF EXISTS (SELECT 1 FROM UserShipAssignments WHERE UserId = @UserId AND ShipCode = @ShipCode)
    BEGIN
        RAISERROR('Ship %s is already assigned to user %d.', 16, 1, @ShipCode, @UserId);
        RETURN;
    END
    
    INSERT INTO UserShipAssignments (UserId, ShipCode, AssignedDate)
    VALUES (@UserId, @ShipCode, GETDATE());
    
    SELECT 
        usa.UserId,
        usa.ShipCode,
        usa.AssignedDate,
        u.Name AS UserName,
        u.Role AS UserRole,
        s.Name AS ShipName,
        s.FiscalYear,
        s.Status AS ShipStatus
    FROM UserShipAssignments usa
    INNER JOIN Users u ON usa.UserId = u.UserId
    INNER JOIN Ships s ON usa.ShipCode = s.Code
    WHERE usa.UserId = @UserId AND usa.ShipCode = @ShipCode;
END;
GO
