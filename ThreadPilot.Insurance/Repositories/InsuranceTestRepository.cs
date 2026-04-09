using ThreadPilot.Insurance.Repositories.Interfaces;

namespace ThreadPilot.Insurance.Repositories;

public class InsuranceTestRepository : IInsuranceRepository
{
    public Task<List<Entities.Insurance>?> GetInsurancesForPersonAsync(string personNumber)
    {
        return Task.FromResult(insurancesByPersonNumber.TryGetValue(personNumber, out var insurances)
            ? insurances
            : null);
    }

    public Task<Entities.Insurance?> GetInsuranceAsync(string insuranceNumber)
    {
        return Task.FromResult(insurancesByPersonNumber.Values
            .SelectMany(i => i)
            .FirstOrDefault(i => i.Number == insuranceNumber));
    }

    private static Dictionary<string, List<Entities.Insurance>> insurancesByPersonNumber = new Dictionary<string, List<Entities.Insurance>>
    {
        {
            "800404-3025", new List<Entities.Insurance>
            {
                new Entities.Insurance
                {
                    Id = Guid.NewGuid(),
                    Number = "1515313131",
                    Type = "Personal Health",
                    ValidUntil = DateTime.UtcNow.AddMonths(10),
                    Price = 600
                },
                new Entities.Insurance
                {
                    Id = Guid.NewGuid(),
                    Number = "1016513215",
                    Type = "Car",
                    ValidUntil = DateTime.UtcNow.AddMonths(7),
                    Price = 130,
                    VehicleRegistrationNumber = "ABC123"
                }
            }
        },
        {
            "920123-4052", new List<Entities.Insurance>
            {
                new Entities.Insurance
                {
                    Id = Guid.NewGuid(),
                    Number = "4528126575",
                    Type = "Pet",
                    ValidUntil = DateTime.UtcNow.AddMonths(10),
                    Price = 600
                },
                new Entities.Insurance
                {
                    Id = Guid.NewGuid(),
                    Number = "1258935648",
                    Type = "Car",
                    ValidUntil = DateTime.UtcNow.AddMonths(9),
                    Price = 150,
                    VehicleRegistrationNumber = "EFG468"
                },
                new Entities.Insurance
                {
                    Id = Guid.NewGuid(),
                    Number = "7525423286",
                    Type = "Home",
                    ValidUntil = DateTime.UtcNow.AddMonths(11),
                    Price = 260
                }
            }
        },
        {
            "750101-1234", new List<Entities.Insurance>
            {
                new Entities.Insurance
                {
                    Id = Guid.NewGuid(),
                    Number = "7894561230",
                    Type = "Home",
                    ValidUntil = DateTime.UtcNow.AddMonths(1),
                    Price = 210
                }
            }
        }
    };
}