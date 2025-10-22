CREATE PROCEDURE sp_GetUserById
    @UserId INT
AS
BEGIN
    SET NOCOUNT ON;
    
    SELECT 
        UserId,
        Name,
        Role
    FROM Users
    WHERE UserId = @UserId;
END;
GO
