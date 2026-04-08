namespace ThreadPilot.Contracts;
public class VehicleDto
{
    public required string RegestrationNumber { get; set; }
    public required string Brand { get; set; }
    public required string Model { get; set; }
    public required int Year { get; set; }
}
