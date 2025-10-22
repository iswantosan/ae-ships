CREATE PROCEDURE sp_GetCrewMemberHistory
    @CrewMemberId VARCHAR(20)
AS
BEGIN
    SET NOCOUNT ON;
    
    SELECT 
        cm.CrewMemberId,
        cm.FirstName,
        cm.LastName,
        cm.BirthDate,
        cm.Nationality,
        s.Code AS ShipCode,
        s.Name AS ShipName,
        r.RankName,
        csh.SignOnDate,
        csh.SignOffDate,
        csh.EndOfContractDate,
        CASE 
            WHEN csh.SignOffDate IS NOT NULL THEN 'Signed Off'
            WHEN CAST(GETDATE() AS DATE) < csh.SignOnDate THEN 'Planned'
            WHEN csh.SignOffDate IS NULL AND CAST(GETDATE() AS DATE) <= csh.EndOfContractDate THEN 'Onboard'
            WHEN csh.SignOffDate IS NULL AND DATEDIFF(DAY, csh.EndOfContractDate, CAST(GETDATE() AS DATE)) > 30 THEN 'Relief Due'
            ELSE 'Unknown'
        END AS Status
    FROM CrewMembers cm
    INNER JOIN CrewServiceHistory csh ON cm.CrewMemberId = csh.CrewMemberId
    INNER JOIN Ships s ON csh.ShipCode = s.Code
    INNER JOIN Ranks r ON csh.RankId = r.RankId
    WHERE cm.CrewMemberId = @CrewMemberId
    ORDER BY csh.SignOnDate DESC;
END;
GO

