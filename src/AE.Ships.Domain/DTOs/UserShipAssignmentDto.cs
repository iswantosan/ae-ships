namespace AE.Ships.Domain.DTOs;

public class UserShipAssignmentDto
{
    public int UserId { get; set; }
    public string ShipCode { get; set; } = string.Empty;
    public DateTime AssignedDate { get; set; }
    public string UserName { get; set; } = string.Empty;
    public string UserRole { get; set; } = string.Empty;
    public string ShipName { get; set; } = string.Empty;
    public string FiscalYear { get; set; } = string.Empty;
    public string ShipStatus { get; set; } = string.Empty;
}
