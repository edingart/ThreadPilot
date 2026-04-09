using Microsoft.Extensions.Options;
using ThreadPilot.Contracts;
using ThreadPilot.Insurance.Options;
using ThreadPilot.Insurance.Services.Interfaces;

namespace ThreadPilot.Insurance.Services;

public class VehicleService : IVehicleService
{
    private readonly Uri vehicleApiAddress;
    private readonly string? vehicleApiVersion;
    private readonly HttpClient httpClient;

    public VehicleService(IOptions<VehicleApiSettings> vehicleApiSettings, HttpClient httpClient)
    {
        this.httpClient = httpClient;

        var apiAddress = vehicleApiSettings.Value.BaseUrl
            ?? throw new ArgumentNullException(nameof(vehicleApiSettings.Value.BaseUrl));

        vehicleApiAddress = new Uri(apiAddress);
        vehicleApiVersion = vehicleApiSettings.Value.Version;
    }

    public async Task<VehicleDto?> GetVehicleAsync(string vehicleRegistrationNumber)
    {
        var uri = new Uri(vehicleApiAddress, vehicleRegistrationNumber);

        var request = new HttpRequestMessage(HttpMethod.Get, uri);
        if (!string.IsNullOrEmpty(vehicleApiVersion))
        {
            request.Headers.Add("X-API-Version", vehicleApiVersion);
        }

        var response = await httpClient.SendAsync(request);

        if (response.IsSuccessStatusCode)
        {
            var vehicle = await response.Content.ReadFromJsonAsync<VehicleDto>();
            return vehicle;
        }
        else
        {
            return null;
        }
    }
}
