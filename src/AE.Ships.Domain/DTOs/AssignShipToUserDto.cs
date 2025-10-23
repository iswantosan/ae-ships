namespace AE.Ships.Domain.DTOs;

public class AssignShipToUserDto
{
    public int UserId { get; set; }
    public string ShipCode { get; set; } = string.Empty;
}
