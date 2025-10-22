CREATE PROCEDURE sp_UpdateUser
    @UserId INT,
    @Name NVARCHAR(200),
    @Role NVARCHAR(100)
AS
BEGIN
    SET NOCOUNT ON;
    
    UPDATE Users
    SET Name = @Name,
        Role = @Role
    WHERE UserId = @UserId;
END;
GO
