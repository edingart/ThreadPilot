
namespace ThreadPilot.Vehicle.Repositories.Interfaces;

public interface IVehicleRepository
{
    Task<Entities.Vehicle?> GetVehicleAsync(string registrationNumber);
}