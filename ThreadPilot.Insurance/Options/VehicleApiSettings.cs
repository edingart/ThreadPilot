namespace ThreadPilot.Insurance.Options;
public sealed class VehicleApiSettings
{
    public required string BaseUrl { get; set; }
    public string? Version { get; set; }

    public static string SectionName = nameof(VehicleApiSettings);
}
