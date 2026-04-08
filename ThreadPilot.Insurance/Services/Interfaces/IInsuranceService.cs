using ThreadPilot.Contracts;

namespace ThreadPilot.Insurance.Services.Interfaces;

public interface IInsuranceService
{
    Task<List<InsuranceDto>?> GetInsurancesByPersonNumberAsync(string personNumber);
}