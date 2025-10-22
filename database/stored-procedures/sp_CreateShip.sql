CREATE PROCEDURE sp_CreateShip
    @Code VARCHAR(20),
    @Name NVARCHAR(200),
    @FiscalYear CHAR(4),
    @Status VARCHAR(20)
AS
BEGIN
    SET NOCOUNT ON;
    
    INSERT INTO Ships (Code, Name, FiscalYear, Status)
    VALUES (@Code, @Name, @FiscalYear, @Status);
END;
GO

