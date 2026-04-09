namespace ThreadPilot.Insurance.Entities;

public class Insurance
{
    public required Guid Id { get; set; }
    public required string Number { get; set; }
    public required string Type { get; set; }
    public required int Price { get; set; }
    public required DateTime ValidUntil { get; set; }

    public string? VehicleRegistrationNumber { get; set; }
}
