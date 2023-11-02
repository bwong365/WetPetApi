using AutoFixture.Xunit2;
using NSubstitute;
using WetPet.AppCore.Common.Enums;
using WetPet.AppCore.Common.Errors;
using WetPet.AppCore.Entities;
using WetPet.AppCore.Interfaces;
using WetPet.AppCore.Services.Queries.GetPetReport;
using WetPet.AppCore.ValueObjects;

namespace WetPet.AppCore.Test.Services.Queries.GetPetReport;

public class GetPetReportQueryTests
{
    private readonly IPetRepository _petRepository;
    private readonly IOwnerRepository _ownerRepository;
    private readonly IWeatherService _weatherService;
    private readonly IPetStatusService _petStatusService;
    private readonly GetPetReportQueryHandler _sut;

    public GetPetReportQueryTests()
    {
        _petRepository = Substitute.For<IPetRepository>();
        _ownerRepository = Substitute.For<IOwnerRepository>();
        _weatherService = Substitute.For<IWeatherService>();
        _petStatusService = Substitute.For<IPetStatusService>();
        _sut = new GetPetReportQueryHandler(_petRepository, _ownerRepository, _weatherService, _petStatusService);
    }

    [Theory]
    [AutoData]
    public async Task Handle_WhenOwnerNotFound_ReturnsOwnerNotFound(string sub)
    {
        var result = await _sut.Handle(new GetPetReportQuery { Sub = sub, PetId = Guid.NewGuid() }, CancellationToken.None);

        Assert.True(result.IsError);
        Assert.Equal(Errors.Owner.NotFound.Code, result.FirstError.Code);
    }

    [Theory, AutoData]
    public async Task Handle_WhenPetNotFound_ReturnsPetNotFound(Owner owner)
    {
        var query = new GetPetReportQuery { PetId = Guid.NewGuid(), Sub = owner.Sub };
        _ownerRepository.GetOwnerAsync(owner.Sub, Arg.Any<CancellationToken>()).Returns(owner);

        var result = await _sut.Handle(query, CancellationToken.None);

        Assert.True(result.IsError);
        Assert.Equal("Pet.NotFound", result.FirstError.Code);
    }

    [Theory, AutoData]
    public async Task Handle_WhenPetDoesNotBelongToOwner_ReturnsPetNotFound(Owner owner, Pet pet)
    {
        pet.OwnerId = Guid.NewGuid();
        owner.Id = Guid.NewGuid();
        var query = new GetPetReportQuery { PetId = pet.Id, Sub = owner.Sub };
        _ownerRepository.GetOwnerAsync(owner.Sub, Arg.Any<CancellationToken>()).Returns(owner);
        _petRepository.GetPetAsync(pet.Id, Arg.Any<CancellationToken>()).Returns(pet);

        var result = await _sut.Handle(query, CancellationToken.None);

        Assert.True(result.IsError);
        Assert.Equal("Pet.NotFound", result.FirstError.Code);
    }

    [Theory, AutoData]
    public async Task Handle_WhenWeatherServiceHasError_ReturnsError(Owner owner, Pet pet)
    {
        pet.OwnerId = owner.Id;
        var query = new GetPetReportQuery { PetId = pet.Id, Sub = owner.Sub };
        _ownerRepository.GetOwnerAsync(owner.Sub, Arg.Any<CancellationToken>()).Returns(owner);
        _petRepository.GetPetAsync(pet.Id, Arg.Any<CancellationToken>()).Returns(pet);
        _weatherService.GetWeatherDataAsync(pet.Location, Arg.Any<CancellationToken>()).Returns(Errors.Location.InvalidLocation);

        var result = await _sut.Handle(query, CancellationToken.None);

        Assert.True(result.IsError);
        Assert.Equal(Errors.Location.InvalidLocation.Code, result.FirstError.Code);
    }

    [Theory, AutoData]
    public async Task Handle_WhenPetStatusServiceHasError_ReturnError(Owner owner, Pet pet)
    {
        pet.OwnerId = owner.Id;
        var query = new GetPetReportQuery { PetId = pet.Id, Sub = owner.Sub };
        _ownerRepository.GetOwnerAsync(owner.Sub, Arg.Any<CancellationToken>()).Returns(owner);
        _petRepository.GetPetAsync(pet.Id, Arg.Any<CancellationToken>()).Returns(pet);
        _weatherService.GetWeatherDataAsync(pet.Location, Arg.Any<CancellationToken>()).Returns(new WeatherData());
        _petStatusService.GetPetStatuses(pet.Species, Arg.Any<WeatherData>()).Returns(Errors.Pet.UnknownSpecies);

        var result = await _sut.Handle(query, CancellationToken.None);

        Assert.True(result.IsError);
        Assert.Equal(Errors.Pet.UnknownSpecies.Code, result.FirstError.Code);
    }

    [Theory, AutoData]
    public async Task Success(Owner owner, Pet pet, WeatherData weatherData)
    {
        pet.OwnerId = owner.Id;
        var query = new GetPetReportQuery { PetId = pet.Id, Sub = owner.Sub };
        _ownerRepository.GetOwnerAsync(owner.Sub, Arg.Any<CancellationToken>()).Returns(owner);
        _petRepository.GetPetAsync(pet.Id, Arg.Any<CancellationToken>()).Returns(pet);
        _weatherService.GetWeatherDataAsync(pet.Location, Arg.Any<CancellationToken>()).Returns(weatherData);
        _petStatusService.GetPetStatuses(pet.Species, Arg.Any<WeatherData>()).Returns(new List<PetStatus>() { PetStatus.Content });

        var result = await _sut.Handle(query, CancellationToken.None);

        Assert.False(result.IsError);
        Assert.NotNull(result.Value);
        Assert.Equal(pet, result.Value.Pet);
        Assert.Equal(weatherData, result.Value.WeatherData);
        Assert.Contains(PetStatus.Content, result.Value.Statuses);
    }
}