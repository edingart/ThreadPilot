using ThreadPilot.Contracts;

namespace ThreadPilot.Insurance.Services.Interfaces;

public interface IVehicleService
{
    Task<VehicleDto?> GetVehicleAsync(string vehicleRegistrationNumber);
}