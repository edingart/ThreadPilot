using Asp.Versioning;
using Microsoft.AspNetCore.Mvc;
using ThreadPilot.Contracts;
using ThreadPilot.Vehicle.Repositories.Interfaces;

namespace ThreadPilot.Vehicle.Controllers;

[ApiController]
[Route("[controller]")]
[ApiVersion("1.0")]
public class VehiclesController : ControllerBase
{
    private readonly ILogger<VehiclesController> logger;
    private readonly IVehicleRepository vehicleRepository;

    public VehiclesController(ILogger<VehiclesController> logger, IVehicleRepository vehicleRepository)
    {
        this.logger = logger;
        this.vehicleRepository = vehicleRepository;
    }

    [HttpGet("{registrationNumber}")]
    [ProducesResponseType<VehicleDto>(StatusCodes.Status200OK)]
    [ProducesResponseType(StatusCodes.Status404NotFound)]
    [ProducesResponseType(StatusCodes.Status400BadRequest)]
    [ProducesResponseType(StatusCodes.Status500InternalServerError)]
    public async Task<ActionResult<VehicleDto>> GetAsync(string registrationNumber)
    {
        try
        {
            if (string.IsNullOrWhiteSpace(registrationNumber))
            {
                return BadRequest("Registration number must be provided.");
            }

            var vehicle = await vehicleRepository.GetVehicleAsync(registrationNumber);
            if (vehicle is null)
            {
                return NotFound();
            }

            return Ok(new VehicleDto()
            { 
                RegestrationNumber = vehicle.RegestrationNumber,
                Brand = vehicle.Brand,
                Model = vehicle.Model,
                Year = vehicle.Year
            });
        }
        catch (Exception e)
        {
            logger.LogError(e, "An error occurred while processing the request for vehicle with registration number {RegistrationNumber}", registrationNumber);
            return StatusCode(StatusCodes.Status500InternalServerError, "An error occurred while processing your request.");
        }
    }
}
