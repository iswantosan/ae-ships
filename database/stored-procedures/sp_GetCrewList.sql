CREATE PROCEDURE sp_GetCrewList
    @ShipCode VARCHAR(20),
    @PageNumber INT = 1,
    @PageSize INT = 10,
    @SortColumn VARCHAR(50) = 'RankOrder',
    @SortDirection VARCHAR(4) = 'ASC',
    @SearchTerm NVARCHAR(200) = NULL
AS
BEGIN
    SET NOCOUNT ON;
    
    DECLARE @Offset INT = (@PageNumber - 1) * @PageSize;
    DECLARE @CurrentDate DATE = CAST(GETDATE() AS DATE);
    
    WITH CrewData AS (
        SELECT 
            r.RankName,
            r.RankOrder,
            cm.CrewMemberId,
            cm.FirstName,
            cm.LastName,
            DATEDIFF(YEAR, cm.BirthDate, @CurrentDate) - 
                CASE 
                    WHEN MONTH(cm.BirthDate) > MONTH(@CurrentDate) 
                        OR (MONTH(cm.BirthDate) = MONTH(@CurrentDate) AND DAY(cm.BirthDate) > DAY(@CurrentDate))
                    THEN 1 
                    ELSE 0 
                END AS Age,
            cm.Nationality,
            csh.SignOnDate,
            CASE 
                WHEN csh.SignOffDate IS NULL AND @CurrentDate <= csh.EndOfContractDate THEN 'Onboard'
                WHEN csh.SignOffDate IS NULL AND DATEDIFF(DAY, csh.EndOfContractDate, @CurrentDate) > 30 THEN 'Relief Due'
                ELSE 'Unknown'
            END AS Status,
            FORMAT(csh.SignOnDate, 'dd MMM yyyy') AS FormattedSignOnDate
        FROM CrewServiceHistory csh
        INNER JOIN CrewMembers cm ON csh.CrewMemberId = cm.CrewMemberId
        INNER JOIN Ranks r ON csh.RankId = r.RankId
        WHERE csh.ShipCode = @ShipCode
            AND csh.SignOffDate IS NULL
            AND csh.SignOnDate <= @CurrentDate
            AND (
                @CurrentDate <= csh.EndOfContractDate 
                OR DATEDIFF(DAY, csh.EndOfContractDate, @CurrentDate) > 30
            )
            AND (
                @SearchTerm IS NULL 
                OR r.RankName LIKE '%' + @SearchTerm + '%'
                OR cm.CrewMemberId LIKE '%' + @SearchTerm + '%'
                OR cm.FirstName LIKE '%' + @SearchTerm + '%'
                OR cm.LastName LIKE '%' + @SearchTerm + '%'
                OR cm.Nationality LIKE '%' + @SearchTerm + '%'
                OR CAST(DATEDIFF(YEAR, cm.BirthDate, @CurrentDate) - 
                        CASE 
                            WHEN MONTH(cm.BirthDate) > MONTH(@CurrentDate) 
                                OR (MONTH(cm.BirthDate) = MONTH(@CurrentDate) AND DAY(cm.BirthDate) > DAY(@CurrentDate))
                            THEN 1 
                            ELSE 0 
                        END AS VARCHAR) LIKE '%' + @SearchTerm + '%'
                OR FORMAT(csh.SignOnDate, 'dd MMM yyyy') LIKE '%' + @SearchTerm + '%'
                OR FORMAT(csh.SignOnDate, 'dd') LIKE '%' + @SearchTerm + '%'
                OR FORMAT(csh.SignOnDate, 'MMM') LIKE '%' + @SearchTerm + '%'
                OR FORMAT(csh.SignOnDate, 'yyyy') LIKE '%' + @SearchTerm + '%'
            )
    ),
    TotalCount AS (
        SELECT COUNT(*) AS TotalRecords FROM CrewData
    )
    SELECT 
        cd.RankName AS [Rank Name],
        cd.CrewMemberId AS [Crew Member ID],
        cd.FirstName AS [First Name],
        cd.LastName AS [Last Name],
        cd.Age,
        cd.Nationality,
        cd.FormattedSignOnDate AS SignOnDate,
        cd.Status,
        tc.TotalRecords
    FROM CrewData cd
    CROSS JOIN TotalCount tc
    ORDER BY 
        CASE WHEN @SortColumn = 'RankName' AND @SortDirection = 'ASC' THEN cd.RankName END ASC,
        CASE WHEN @SortColumn = 'RankName' AND @SortDirection = 'DESC' THEN cd.RankName END DESC,
        CASE WHEN @SortColumn = 'RankOrder' AND @SortDirection = 'ASC' THEN cd.RankOrder END ASC,
        CASE WHEN @SortColumn = 'RankOrder' AND @SortDirection = 'DESC' THEN cd.RankOrder END DESC,
        CASE WHEN @SortColumn = 'CrewMemberId' AND @SortDirection = 'ASC' THEN cd.CrewMemberId END ASC,
        CASE WHEN @SortColumn = 'CrewMemberId' AND @SortDirection = 'DESC' THEN cd.CrewMemberId END DESC,
        CASE WHEN @SortColumn = 'FirstName' AND @SortDirection = 'ASC' THEN cd.FirstName END ASC,
        CASE WHEN @SortColumn = 'FirstName' AND @SortDirection = 'DESC' THEN cd.FirstName END DESC,
        CASE WHEN @SortColumn = 'LastName' AND @SortDirection = 'ASC' THEN cd.LastName END ASC,
        CASE WHEN @SortColumn = 'LastName' AND @SortDirection = 'DESC' THEN cd.LastName END DESC,
        CASE WHEN @SortColumn = 'Age' AND @SortDirection = 'ASC' THEN cd.Age END ASC,
        CASE WHEN @SortColumn = 'Age' AND @SortDirection = 'DESC' THEN cd.Age END DESC,
        CASE WHEN @SortColumn = 'Nationality' AND @SortDirection = 'ASC' THEN cd.Nationality END ASC,
        CASE WHEN @SortColumn = 'Nationality' AND @SortDirection = 'DESC' THEN cd.Nationality END DESC,
        CASE WHEN @SortColumn = 'SignOnDate' AND @SortDirection = 'ASC' THEN cd.SignOnDate END ASC,
        CASE WHEN @SortColumn = 'SignOnDate' AND @SortDirection = 'DESC' THEN cd.SignOnDate END DESC,
        CASE WHEN @SortColumn = 'Status' AND @SortDirection = 'ASC' THEN cd.Status END ASC,
        CASE WHEN @SortColumn = 'Status' AND @SortDirection = 'DESC' THEN cd.Status END DESC
    OFFSET @Offset ROWS
    FETCH NEXT @PageSize ROWS ONLY;
END;
GO

