namespace ThreadPilot.Insurance.Repositories.Interfaces;

public interface IInsuranceRepository
{
    Task<List<Entities.Insurance>?> GetInsurancesForPersonAsync(string personNumber);
    Task<Entities.Insurance?> GetInsuranceAsync(string insuranceNumber);
}