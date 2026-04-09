using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;
using Microsoft.Extensions.Logging;
using Moq;
using ThreadPilot.Contracts;
using ThreadPilot.Vehicle.Controllers;
using ThreadPilot.Vehicle.Repositories.Interfaces;

namespace ThreadPilot.Test.UnitTests.Vehicle;

public class VehiclesControllerTests
{
    private readonly Mock<ILogger<VehiclesController>> loggerMock;
    private readonly Mock<IVehicleRepository> vehicleRepositoryMock;
    private readonly VehiclesController sut;

    public VehiclesControllerTests()
    {
        loggerMock = new Mock<ILogger<VehiclesController>>();
        vehicleRepositoryMock = new Mock<IVehicleRepository>();
        sut = new VehiclesController(loggerMock.Object, vehicleRepositoryMock.Object);
    }

    [Theory]
    [InlineData(null)]
    [InlineData("")]
    [InlineData(" ")]
    [InlineData("   ")]
    public async Task GetAsync_RegistrationNumberMissing_ReturnsBadRequest(string? registrationNumber)
    {
        // Act
        var result = await sut.GetAsync(registrationNumber!);

        // Assert
        var badRequest = Assert.IsType<BadRequestObjectResult>(result.Result);
        Assert.Equal(StatusCodes.Status400BadRequest, badRequest.StatusCode);
        Assert.Equal("Registration number must be provided.", badRequest.Value);

        vehicleRepositoryMock.Verify(
            r => r.GetVehicleAsync(It.IsAny<string>()),
            Times.Never);
    }

    [Fact]
    public async Task GetAsync_VehicleNotFound_ReturnsNotFound()
    {
        // Arrange
        var regNo = "ABC123";
        vehicleRepositoryMock
            .Setup(r => r.GetVehicleAsync(regNo))
            .ReturnsAsync((ThreadPilot.Vehicle.Entities.Vehicle?)null);

        // Act
        var result = await sut.GetAsync(regNo);

        // Assert
        Assert.IsType<NotFoundResult>(result.Result);

        vehicleRepositoryMock.Verify(
            r => r.GetVehicleAsync(regNo),
            Times.Once);
    }

    [Fact]
    public async Task GetAsync_VehicleFound_MapsToDtoAndReturnsOk()
    {
        // Arrange
        var regNo = "ABC123";
        var vehicle = new ThreadPilot.Vehicle.Entities.Vehicle
        {
            Id = Guid.NewGuid(),
            RegestrationNumber = regNo,
            Brand = "Volvo",
            Model = "XC60",
            Year = 2020
        };

        vehicleRepositoryMock
            .Setup(r => r.GetVehicleAsync(regNo))
            .ReturnsAsync(vehicle);

        // Act
        var result = await sut.GetAsync(regNo);

        // Assert
        var okResult = Assert.IsType<OkObjectResult>(result.Result);
        Assert.Equal(StatusCodes.Status200OK, okResult.StatusCode);

        var dto = Assert.IsType<VehicleDto>(okResult.Value);
        Assert.Equal(regNo, dto.RegestrationNumber);
        Assert.Equal("Volvo", dto.Brand);
        Assert.Equal("XC60", dto.Model);
        Assert.Equal(2020, dto.Year);

        vehicleRepositoryMock.Verify(
            r => r.GetVehicleAsync(regNo),
            Times.Once);
    }

    [Fact]
    public async Task GetAsync_RepositoryThrowsException_ReturnsInternalServerError()
    {
        // Arrange
        var regNo = "ABC123";

        vehicleRepositoryMock
            .Setup(r => r.GetVehicleAsync(regNo))
            .ThrowsAsync(new Exception("DB error"));

        // Act
        var result = await sut.GetAsync(regNo);

        // Assert
        var objectResult = Assert.IsType<ObjectResult>(result.Result);
        Assert.Equal(StatusCodes.Status500InternalServerError, objectResult.StatusCode);
        Assert.Equal("An error occurred while processing your request.", objectResult.Value);

        vehicleRepositoryMock.Verify(
            r => r.GetVehicleAsync(regNo),
            Times.Once);
    }
}