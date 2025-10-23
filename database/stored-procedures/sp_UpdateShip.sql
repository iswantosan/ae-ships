CREATE PROCEDURE sp_UpdateShip
    @Code VARCHAR(20),
    @Name NVARCHAR(200),
    @FiscalYear CHAR(4),
    @Status VARCHAR(20)
AS
BEGIN
    SET NOCOUNT ON;
    
    UPDATE Ships
    SET Name = @Name,
        FiscalYear = @FiscalYear,
        Status = @Status
    WHERE Code = @Code;
END;
GO


