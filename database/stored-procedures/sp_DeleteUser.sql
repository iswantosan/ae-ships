CREATE PROCEDURE sp_DeleteUser
    @UserId INT
AS
BEGIN
    SET NOCOUNT ON;
    
    DELETE FROM Users
    WHERE UserId = @UserId;
END;
GO
