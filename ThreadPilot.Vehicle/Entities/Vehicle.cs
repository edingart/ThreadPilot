namespace ThreadPilot.Vehicle.Entities;

public class Vehicle
{
    public required Guid Id { get; set; }
    public required string RegestrationNumber { get; set; }
    public required string Brand { get; set; }
    public required string Model { get; set; }
    public required int Year { get; set; }
}
