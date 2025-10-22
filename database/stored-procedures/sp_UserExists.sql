CREATE PROCEDURE sp_UserExists
    @UserId INT
AS
BEGIN
    SET NOCOUNT ON;
    
    SELECT CASE 
        WHEN EXISTS (SELECT 1 FROM Users WHERE UserId = @UserId) 
        THEN 1 
        ELSE 0 
    END;
END;
GO
