using ThreadPilot.Vehicle.Repositories.Interfaces;

namespace ThreadPilot.Vehicle.Repositories;
public class VehicleRepository : IVehicleRepository
{
    public Task<Entities.Vehicle?> GetVehicleAsync(string registrationNumber)
    {
        return Task.FromResult(vehiclesByRegistrationNumber.TryGetValue(registrationNumber.ToLowerInvariant(), out var vehicle) ? vehicle : null);
    }

    private static Dictionary<string, Entities.Vehicle> vehiclesByRegistrationNumber = new Dictionary<string, Entities.Vehicle>
    {
        {
            "abc123", new Entities.Vehicle
            {
                Id = Guid.NewGuid(),
                Brand = "Volvo",
                Model = "V90",
                RegestrationNumber = "ABC123",
                Year = 2018
            }
        },
        {
            "efg468", new Entities.Vehicle
            {
                Id = Guid.NewGuid(),
                Brand = "Tesla",
                Model = "Model 3",
                RegestrationNumber = "EFG468",
                Year = 2020
            }
        }
    };
}
