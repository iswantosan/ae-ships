CREATE PROCEDURE sp_GetUserShipAssignments
    @UserId INT = NULL,
    @ShipCode VARCHAR(20) = NULL
AS
BEGIN
    SET NOCOUNT ON;
    
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
    WHERE 
        (@UserId IS NULL OR usa.UserId = @UserId)
        AND (@ShipCode IS NULL OR usa.ShipCode = @ShipCode)
    ORDER BY usa.AssignedDate DESC;
END;
GO
