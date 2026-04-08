using ThreadPilot.Contracts;
using ThreadPilot.Insurance.Repositories.Interfaces;
using ThreadPilot.Insurance.Services.Interfaces;

namespace ThreadPilot.Insurance.Services;
public class InsuranceService : IInsuranceService
{
    private readonly IInsuranceRepository repository;
    private readonly IVehicleService vehicleService;

    public InsuranceService(IInsuranceRepository repository, IVehicleService vehicleService)
    {
        this.repository = repository;
        this.vehicleService = vehicleService;
    }

    public async Task<List<InsuranceDto>?> GetInsurancesByPersonNumberAsync(string personNumber)
    {
        if (!IsValidPersonNumber(personNumber))
        {
            throw new ArgumentException("Invalid person number format. Please use the format YYMMDD-XXXX", nameof(personNumber));
        }

        var insurances = await repository.GetInsurancesForPersonAsync(personNumber);

        if (insurances is null || insurances.Count == 0)
        {
            return null;
        }

        var insuranceDtos = new List<InsuranceDto>();

        foreach (var insurance in insurances)
        {
            var dto = new InsuranceDto
            {
                Number = insurance.Number,
                Type = insurance.Type,
                ValidUntil = DateOnly.FromDateTime(insurance.ValidUntil),
                Price = insurance.Price
            };

            if (insurance.VehicleRegistrationNumber is not null)
            {
                var vehicleInfo = await vehicleService.GetVehicleAsync(insurance.VehicleRegistrationNumber);
                if (vehicleInfo != null)
                {
                    dto.VehicleData = vehicleInfo;
                }
                else
                {
                    dto.VehicleData = new()
                    { Brand = "Unknown", Model = "Unknown", RegestrationNumber = insurance.VehicleRegistrationNumber, Year = 0 };
                }
            }

            insuranceDtos.Add(dto);
        }

        return insuranceDtos;
    }

    private bool IsValidPersonNumber(string personNumber)
        => personNumber is not null
        && personNumber is { Length: 11 }
        && (personNumber[6] is '-' or '+');
    // Todo: Add control digit validation
}
