namespace AE.Ships.Domain.DTOs;

public class UnassignShipFromUserDto
{
    public int UserId { get; set; }
    public string ShipCode { get; set; } = string.Empty;
}
