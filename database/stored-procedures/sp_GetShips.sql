CREATE PROCEDURE sp_GetShips
    @Status VARCHAR(20) = NULL,
    @UserId INT = NULL
AS
BEGIN
    SET NOCOUNT ON;
    
    SELECT 
        s.Code,
        s.Name,
        s.FiscalYear,
        s.Status
    FROM Ships s
    WHERE 
        (@Status IS NULL OR s.Status = @Status)
        AND (
            @UserId IS NULL 
            OR EXISTS (
                SELECT 1 
                FROM UserShipAssignments usa 
                WHERE usa.ShipCode = s.Code 
                  AND usa.UserId = @UserId
            )
        )
    ORDER BY s.Code;
END;
GO

