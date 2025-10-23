namespace AE.Ships.Domain.DTOs;

public class CrewListDto
{
    public string RankName { get; set; } = string.Empty;
    public string CrewMemberId { get; set; } = string.Empty;
    public string FirstName { get; set; } = string.Empty;
    public string LastName { get; set; } = string.Empty;
    public int Age { get; set; }
    public string Nationality { get; set; } = string.Empty;
    public string SignOnDate { get; set; } = string.Empty;
    public string Status { get; set; } = string.Empty;
}

public class CrewListRequestDto
{
    public string ShipCode { get; set; } = string.Empty;
    public int PageNumber { get; set; } = 1;
    public int PageSize { get; set; } = 10;
    public string SortColumn { get; set; } = "RankOrder";
    public string SortDirection { get; set; } = "ASC";
    public string? SearchTerm { get; set; }
}

public class CrewMemberHistoryDto
{
    public string CrewMemberId { get; set; } = string.Empty;
    public string FirstName { get; set; } = string.Empty;
    public string LastName { get; set; } = string.Empty;
    public DateTime BirthDate { get; set; }
    public string Nationality { get; set; } = string.Empty;
    public string ShipCode { get; set; } = string.Empty;
    public string ShipName { get; set; } = string.Empty;
    public string RankName { get; set; } = string.Empty;
    public DateTime SignOnDate { get; set; }
    public DateTime? SignOffDate { get; set; }
    public DateTime EndOfContractDate { get; set; }
    public string Status { get; set; } = string.Empty;
}
