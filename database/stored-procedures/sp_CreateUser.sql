CREATE PROCEDURE sp_CreateUser
    @Name NVARCHAR(200),
    @Role NVARCHAR(100),
    @UserId INT OUTPUT
AS
BEGIN
    SET NOCOUNT ON;
    
    INSERT INTO Users (Name, Role)
    VALUES (@Name, @Role);
    
    SET @UserId = SCOPE_IDENTITY();
END;
GO
