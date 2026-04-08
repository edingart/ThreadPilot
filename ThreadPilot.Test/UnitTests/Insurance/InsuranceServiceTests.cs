using Moq;
using ThreadPilot.Contracts;
using ThreadPilot.Insurance.Repositories.Interfaces;
using ThreadPilot.Insurance.Services;
using ThreadPilot.Insurance.Services.Interfaces;

namespace ThreadPilot.Test.UnitTests.Insurance;
public class InsuranceServiceTests
{
    private readonly Mock<IInsuranceRepository> _repositoryMock;
    private readonly Mock<IVehicleService> _vehicleServiceMock;
    private readonly InsuranceService _sut;

    public InsuranceServiceTests()
    {
        _repositoryMock = new Mock<IInsuranceRepository>();
        _vehicleServiceMock = new Mock<IVehicleService>();
        _sut = new InsuranceService(_repositoryMock.Object, _vehicleServiceMock.Object);
    }

    [Theory]
    [InlineData(null!)]
    [InlineData("")]
    [InlineData("1234567890")]     // too short
    [InlineData("123456789012")]   // too long
    [InlineData("1234567890A")]    // wrong separator
    public async Task GetInsurancesByPersonNumberAsync_InvalidPersonNumber_ThrowsArgumentException(string personNumber)
    {
        // Act
        async Task Act() => await _sut.GetInsurancesByPersonNumberAsync(personNumber);

        // Assert
        var ex = await Assert.ThrowsAsync<ArgumentException>(Act);
        Assert.Equal("personNumber", ex.ParamName);
        Assert.Contains("Invalid person number format", ex.Message);
    }

    [Fact]
    public async Task GetInsurancesByPersonNumberAsync_NoInsurances_ReturnsNull()
    {
        // Arrange
        var personNumber = "901231-1234";
        _repositoryMock
            .Setup(r => r.GetInsurancesForPersonAsync(personNumber))
            .ReturnsAsync((List<ThreadPilot.Insurance.Entities.Insurance>)null!);

        // Act
        var result = await _sut.GetInsurancesByPersonNumberAsync(personNumber);

        // Assert
        Assert.Null(result);
        _repositoryMock.Verify(r => r.GetInsurancesForPersonAsync(personNumber), Times.Once);
        _vehicleServiceMock.VerifyNoOtherCalls();
    }

    [Fact]
    public async Task GetInsurancesByPersonNumberAsync_EmptyList_ReturnsNull()
    {
        // Arrange
        var personNumber = "901231-1234";
        _repositoryMock
            .Setup(r => r.GetInsurancesForPersonAsync(personNumber))
            .ReturnsAsync(new List<ThreadPilot.Insurance.Entities.Insurance>());

        // Act
        var result = await _sut.GetInsurancesByPersonNumberAsync(personNumber);

        // Assert
        Assert.Null(result);
        _repositoryMock.Verify(r => r.GetInsurancesForPersonAsync(personNumber), Times.Once);
        _vehicleServiceMock.VerifyNoOtherCalls();
    }

    [Fact]
    public async Task GetInsurancesByPersonNumberAsync_NoVehicleRegistration_MapsBasicFieldsOnly()
    {
        // Arrange
        var personNumber = "901231-1234";
        var insuranceDate = new DateTime(2030, 1, 15);

        var insurance = new ThreadPilot.Insurance.Entities.Insurance
        {
            Id = Guid.NewGuid(),
            Number = "INS-123",
            Type = "Home",
            ValidUntil = insuranceDate,
            Price = 999,
            VehicleRegistrationNumber = null
        };

        _repositoryMock
            .Setup(r => r.GetInsurancesForPersonAsync(personNumber))
            .ReturnsAsync(new List<ThreadPilot.Insurance.Entities.Insurance> { insurance });

        // Act
        var result = await _sut.GetInsurancesByPersonNumberAsync(personNumber);

        // Assert
        Assert.NotNull(result);
        var dto = Assert.Single(result);
        Assert.Equal("INS-123", dto.Number);
        Assert.Equal("Home", dto.Type);
        Assert.Equal(DateOnly.FromDateTime(insuranceDate), dto.ValidUntil);
        Assert.Equal(999, dto.Price);
        Assert.Null(dto.VehicleData);

        _vehicleServiceMock.VerifyNoOtherCalls();
    }

    [Fact]
    public async Task GetInsurancesByPersonNumberAsync_WithVehicle_UsesVehicleServiceResult()
    {
        // Arrange
        var personNumber = "901231-1234";
        var insurance = new ThreadPilot.Insurance.Entities.Insurance
        {
            Id = Guid.NewGuid(),
            Number = "INS-456",
            Type = "Car",
            ValidUntil = new DateTime(2031, 5, 1),
            Price = 1234,
            VehicleRegistrationNumber = "ABC123"
        };

        var vehicleDto = new Contracts.VehicleDto
        {
            Brand = "Volvo",
            Model = "XC60",
            RegestrationNumber = "ABC123",
            Year = 2020
        };

        _repositoryMock
            .Setup(r => r.GetInsurancesForPersonAsync(personNumber))
            .ReturnsAsync(new List<ThreadPilot.Insurance.Entities.Insurance> { insurance });

        _vehicleServiceMock
            .Setup(v => v.GetVehicleAsync("ABC123"))
            .ReturnsAsync(vehicleDto);

        // Act
        var result = await _sut.GetInsurancesByPersonNumberAsync(personNumber);

        // Assert
        var dto = Assert.Single(result);
        Assert.NotNull(dto.VehicleData);
        Assert.Equal("Volvo", dto.VehicleData.Brand);
        Assert.Equal("XC60", dto.VehicleData.Model);
        Assert.Equal("ABC123", dto.VehicleData.RegestrationNumber);
        Assert.Equal(2020, dto.VehicleData.Year);

        _vehicleServiceMock.Verify(v => v.GetVehicleAsync("ABC123"), Times.Once);
    }

    [Fact]
    public async Task GetInsurancesByPersonNumberAsync_WithVehicle_WhenLookupReturnsNull_UsesUnknownVehicleDefaults()
    {
        // Arrange
        var personNumber = "901231-1234";
        var insurance = new ThreadPilot.Insurance.Entities.Insurance
        {
            Id = Guid.NewGuid(),
            Number = "INS-789",
            Type = "Car",
            ValidUntil = new DateTime(2032, 7, 10),
            Price = 2000,
            VehicleRegistrationNumber = "XYZ987"
        };

        _repositoryMock
            .Setup(r => r.GetInsurancesForPersonAsync(personNumber))
            .ReturnsAsync(new List<ThreadPilot.Insurance.Entities.Insurance> { insurance });

        _vehicleServiceMock
            .Setup(v => v.GetVehicleAsync("XYZ987"))
            .ReturnsAsync((VehicleDto)null!);

        // Act
        var result = await _sut.GetInsurancesByPersonNumberAsync(personNumber);

        // Assert
        var dto = Assert.Single(result);
        Assert.NotNull(dto.VehicleData);
        Assert.Equal("Unknown", dto.VehicleData.Brand);
        Assert.Equal("Unknown", dto.VehicleData.Model);
        Assert.Equal("XYZ987", dto.VehicleData.RegestrationNumber);
        Assert.Equal(0, dto.VehicleData.Year);

        _vehicleServiceMock.Verify(v => v.GetVehicleAsync("XYZ987"), Times.Once);
    }
}