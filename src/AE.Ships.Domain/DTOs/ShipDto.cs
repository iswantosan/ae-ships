namespace AE.Ships.Domain.DTOs;

public class ShipDto
{
    public string Code { get; set; } = string.Empty;
    public string Name { get; set; } = string.Empty;
    public string FiscalYear { get; set; } = string.Empty;
    public string Status { get; set; } = string.Empty;
}

public class CreateShipDto
{
    public string Code { get; set; } = string.Empty;
    public string Name { get; set; } = string.Empty;
    public string FiscalYear { get; set; } = string.Empty;
    public string Status { get; set; } = string.Empty;
}

public class UpdateShipDto
{
    public string Code { get; set; } = string.Empty;
    public string Name { get; set; } = string.Empty;
    public string FiscalYear { get; set; } = string.Empty;
    public string Status { get; set; } = string.Empty;
}


