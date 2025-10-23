namespace AE.Ships.Domain.DTOs;

public class UserShipAssignmentDto
{
    public int UserId { get; set; }
    public string ShipCode { get; set; }
    public DateTime AssignedDate { get; set; }
    public string UserName { get; set; }
    public string UserRole { get; set; }
    public string ShipName { get; set; }
    public string FiscalYear { get; set; }
    public string ShipStatus { get; set; }
}
