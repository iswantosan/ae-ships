CREATE PROCEDURE sp_GetShipsByUser
    @UserId INT
AS
BEGIN
    SET NOCOUNT ON;
    
    SELECT 
        s.Code,
        s.Name,
        s.FiscalYear,
        s.Status,
        usa.AssignedDate
    FROM Ships s
    INNER JOIN UserShipAssignments usa ON s.Code = usa.ShipCode
    WHERE usa.UserId = @UserId
    ORDER BY s.Code;
END;
GO

