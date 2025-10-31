CREATE FUNCTION fn_CalculateCrewStatus(
    @SignOnDate DATE,
    @SignOffDate DATE,
    @EndOfContractDate DATE,
    @CurrentDate DATE
)
RETURNS VARCHAR(20)
AS
BEGIN
    DECLARE @Status VARCHAR(20);
    
    IF @SignOffDate IS NOT NULL
        SET @Status = 'Signed Off';
    ELSE IF @SignOnDate > @CurrentDate
        SET @Status = 'Planned';
    ELSE IF @EndOfContractDate < DATEADD(DAY, -30, @CurrentDate)
        SET @Status = 'Relief Due';
    ELSE
        SET @Status = 'Onboard';
    
    RETURN @Status;
END;
GO

