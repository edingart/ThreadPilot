using System.Text.Json.Serialization;

namespace ThreadPilot.Contracts;

public class InsuranceDto
{
    public required string Number { get; set; }
    public required string Type { get; set; }
    public required DateOnly ValidUntil { get; set; }
    public required int Price { get; set; }

    [JsonIgnore(Condition = JsonIgnoreCondition.WhenWritingNull)]
    public VehicleDto? VehicleData { get; set; }
}
