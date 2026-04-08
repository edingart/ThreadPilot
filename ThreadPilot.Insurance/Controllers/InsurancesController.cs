using Asp.Versioning;
using Microsoft.AspNetCore.Mvc;
using ThreadPilot.Contracts;
using ThreadPilot.Insurance.Services.Interfaces;

namespace ThreadPilot.Insurance.Controllers;

[ApiController]
[Route("[controller]")]
[ApiVersion("1.0")]
public class InsurancesController : ControllerBase
{
    private readonly ILogger<InsurancesController> logger;
    private readonly IInsuranceService insuranceService;

    public InsurancesController(ILogger<InsurancesController> logger, IInsuranceService insuranceService)
    {
        this.logger = logger;
        this.insuranceService = insuranceService;
    }

    [HttpGet("{personNumber}")]
    [ProducesResponseType<InsuranceDto>(StatusCodes.Status200OK)]
    [ProducesResponseType(StatusCodes.Status404NotFound)]
    [ProducesResponseType(StatusCodes.Status400BadRequest)]
    [ProducesResponseType(StatusCodes.Status500InternalServerError)]
    public async Task<ActionResult<List<InsuranceDto>>> GetAsync([FromRoute] string personNumber)
    {
        try
        {
            if (string.IsNullOrWhiteSpace(personNumber))
            {
                return BadRequest("Personnumber must be provided.");
            }

            var insuranceDto = await insuranceService.GetInsurancesByPersonNumberAsync(personNumber);
            if (insuranceDto is null)
            {
                return NotFound();
            }

            return Ok(insuranceDto);
        }
        catch (ArgumentException ex)
        {
            return BadRequest(ex.Message);
        }
        catch (Exception e)
        {
            logger.LogError(e, "An error occurred while getting insurance for person number {PersonNumber}", personNumber);
            return StatusCode(StatusCodes.Status500InternalServerError, "An error occurred while processing your request.");
        }
    }
}
