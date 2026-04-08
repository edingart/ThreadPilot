using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;
using Microsoft.Extensions.Logging;
using Moq;
using ThreadPilot.Contracts;
using ThreadPilot.Insurance.Controllers;
using ThreadPilot.Insurance.Services.Interfaces;

namespace ThreadPilot.Test.UnitTests.Insurance;
public class InsurancesControllerTests
{
    private readonly Mock<ILogger<InsurancesController>> loggerMock;
    private readonly Mock<IInsuranceService> insuranceServiceMock;
    private readonly InsurancesController sut;

    public InsurancesControllerTests()
    {
        loggerMock = new Mock<ILogger<InsurancesController>>();
        insuranceServiceMock = new Mock<IInsuranceService>();
        sut = new InsurancesController(loggerMock.Object, insuranceServiceMock.Object);
    }

    [Theory]
    [InlineData(null)]
    [InlineData("")]
    [InlineData(" ")]
    public async Task GetAsync_PersonNumberMissing_ReturnsBadRequest(string personNumber)
    {
        // Act
        var result = await sut.GetAsync(personNumber);

        // Assert
        var badRequest = Assert.IsType<BadRequestObjectResult>(result.Result);
        Assert.Equal(StatusCodes.Status400BadRequest, badRequest.StatusCode);
        Assert.Equal("Personnumber must be provided.", badRequest.Value);

        insuranceServiceMock.Verify(
            s => s.GetInsurancesByPersonNumberAsync(It.IsAny<string>()),
            Times.Never);
    }

    [Fact]
    public async Task GetAsync_ServiceReturnsNull_ReturnsNotFound()
    {
        // Arrange
        var personNumber = "901231-1234";

        insuranceServiceMock
            .Setup(s => s.GetInsurancesByPersonNumberAsync(personNumber))
            .ReturnsAsync((List<InsuranceDto>?)null);

        // Act
        var result = await sut.GetAsync(personNumber);

        // Assert
        Assert.IsType<NotFoundResult>(result.Result);

        insuranceServiceMock.Verify(
            s => s.GetInsurancesByPersonNumberAsync(personNumber),
            Times.Once);
    }

    [Fact]
    public async Task GetAsync_ServiceReturnsInsurances_ReturnsOkWithPayload()
    {
        // Arrange
        var personNumber = "901231-1234";
        var insurances = new List<InsuranceDto>
        {
            new InsuranceDto
            {
                Number = "INS-001",
                Type = "Car",
                ValidUntil = new DateOnly(2030, 12, 31),
                Price = 1234
            },
            new InsuranceDto
            {
                Number = "INS-002",
                Type = "Home",
                ValidUntil = new DateOnly(2031, 6, 30),
                Price = 2500
            }
        };

        insuranceServiceMock
            .Setup(s => s.GetInsurancesByPersonNumberAsync(personNumber))
            .ReturnsAsync(insurances);

        // Act
        var result = await sut.GetAsync(personNumber);

        // Assert
        var okResult = Assert.IsType<OkObjectResult>(result.Result);
        Assert.Equal(StatusCodes.Status200OK, okResult.StatusCode);

        var payload = Assert.IsType<List<InsuranceDto>>(okResult.Value);
        Assert.Equal(2, payload.Count);
        Assert.Equal("INS-001", payload[0].Number);
        Assert.Equal("INS-002", payload[1].Number);

        insuranceServiceMock.Verify(
            s => s.GetInsurancesByPersonNumberAsync(personNumber),
            Times.Once);
    }

    [Theory]
    [InlineData("invalid1234")]
    [InlineData("19900101-0101")]
    [InlineData("9001010101")]
    public async Task GetAsync_ServiceThrowsArgumentException_ReturnsBadRequestWithMessage(string personNumber)
    {
        // Arrange
        insuranceServiceMock
            .Setup(s => s.GetInsurancesByPersonNumberAsync(personNumber))
            .ThrowsAsync(new ArgumentException("Invalid person number format. Please use the format YYMMDD-XXXX"));

        // Act
        var result = await sut.GetAsync(personNumber);

        // Assert
        var badRequest = Assert.IsType<BadRequestObjectResult>(result.Result);
        Assert.Equal(StatusCodes.Status400BadRequest, badRequest.StatusCode);
        Assert.Equal("Invalid person number format. Please use the format YYMMDD-XXXX", badRequest.Value);

        insuranceServiceMock.Verify(
            s => s.GetInsurancesByPersonNumberAsync(personNumber),
            Times.Once);
    }

    [Fact]
    public async Task GetAsync_ServiceThrowsUnexpectedException_ReturnsInternalServerError()
    {
        // Arrange
        var personNumber = "901231-1234";

        insuranceServiceMock
            .Setup(s => s.GetInsurancesByPersonNumberAsync(personNumber))
            .ThrowsAsync(new Exception("Database is down"));

        // Act
        var result = await sut.GetAsync(personNumber);

        // Assert
        var objectResult = Assert.IsType<ObjectResult>(result.Result);
        Assert.Equal(StatusCodes.Status500InternalServerError, objectResult.StatusCode);
        Assert.Equal("An error occurred while processing your request.", objectResult.Value);

        insuranceServiceMock.Verify(
            s => s.GetInsurancesByPersonNumberAsync(personNumber),
            Times.Once);
    }
}