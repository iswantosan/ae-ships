CREATE PROCEDURE sp_GetAllUsers
AS
BEGIN
    SET NOCOUNT ON;
    
    SELECT 
        UserId,
        Name,
        Role
    FROM Users
    ORDER BY Name;
END;
GO
