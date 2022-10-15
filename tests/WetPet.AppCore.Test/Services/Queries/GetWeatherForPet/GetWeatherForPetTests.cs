using AutoFixture.Xunit2;
using Moq;
using WetPet.AppCore.Common.Enums;
using WetPet.AppCore.Common.Errors;
using WetPet.AppCore.Entities;
using WetPet.AppCore.Interfaces;
using WetPet.AppCore.Services.Queries.GetWeatherForPet;
using WetPet.AppCore.ValueObjects;

namespace WetPet.AppCore.Test.Services.Queries.GetWeatherForPet;

public class GetPetReportQueryTests
{
    private readonly Mock<IPetRepository> _petRepository;
    private readonly Mock<IOwnerRepository> _ownerRepository;
    private readonly Mock<IWeatherService> _weatherService;
    private readonly Mock<IPetStatusService> _petStatusService;
    private readonly GetWeatherForPetQueryHandler _sut;

    public GetPetReportQueryTests()
    {
        _petRepository = new Mock<IPetRepository>();
        _ownerRepository = new Mock<IOwnerRepository>();
        _weatherService = new Mock<IWeatherService>();
        _petStatusService = new Mock<IPetStatusService>();
        _sut = new GetWeatherForPetQueryHandler(_petRepository.Object, _ownerRepository.Object, _weatherService.Object, _petStatusService.Object);
    }

    [Theory]
    [AutoData]
    public async Task Handle_WhenOwnerNotFound_ReturnsOwnerNotFound(string sub)
    {
        var result = await _sut.Handle(new GetWeatherForPetQuery { Sub = sub, PetId = Guid.NewGuid() }, CancellationToken.None);

        Assert.True(result.IsError);
        Assert.Equal(Errors.Owner.NotFound.Code, result.FirstError.Code);
    }

    [Theory, AutoData]
    public async Task Handle_WhenPetNotFound_ReturnsPetNotFound(Owner owner)
    {
        var query = new GetWeatherForPetQuery { PetId = Guid.NewGuid(), Sub = owner.Sub };
        _ownerRepository.Setup(x => x.GetOwnerAsync(owner.Sub, It.IsAny<CancellationToken>())).ReturnsAsync(owner);

        var result = await _sut.Handle(query, CancellationToken.None);

        Assert.True(result.IsError);
        Assert.Equal("Pet.NotFound", result.FirstError.Code);
    }

    [Theory, AutoData]
    public async Task Handle_WhenPetDoesNotBelongToOwner_ReturnsPetNotFound(Owner owner, Pet pet)
    {
        pet.OwnerId = Guid.NewGuid();
        owner.Id = Guid.NewGuid();
        var query = new GetWeatherForPetQuery { PetId = pet.Id, Sub = owner.Sub };
        _ownerRepository.Setup(x => x.GetOwnerAsync(owner.Sub, It.IsAny<CancellationToken>())).ReturnsAsync(owner);
        _petRepository.Setup(x => x.GetPetAsync(pet.Id, It.IsAny<CancellationToken>())).ReturnsAsync(pet);

        var result = await _sut.Handle(query, CancellationToken.None);

        Assert.True(result.IsError);
        Assert.Equal("Pet.NotFound", result.FirstError.Code);
    }

    [Theory, AutoData]
    public async Task Handle_WhenWeatherServiceHasError_ReturnsError(Owner owner, Pet pet)
    {
        pet.OwnerId = owner.Id;
        var query = new GetWeatherForPetQuery { PetId = pet.Id, Sub = owner.Sub };
        _ownerRepository.Setup(x => x.GetOwnerAsync(owner.Sub, It.IsAny<CancellationToken>())).ReturnsAsync(owner);
        _petRepository.Setup(x => x.GetPetAsync(query.PetId, It.IsAny<CancellationToken>())).ReturnsAsync(pet);
        _weatherService.Setup(x => x.GetWeatherDataAsync(pet.Location, It.IsAny<CancellationToken>())).ReturnsAsync(Errors.Location.InvalidLocation);

        var result = await _sut.Handle(query, CancellationToken.None);

        Assert.True(result.IsError);
        Assert.Equal(Errors.Location.InvalidLocation.Code, result.FirstError.Code);
    }

    [Theory, AutoData]
    public async Task Handle_WhenPetStatusServiceHasError_ReturnError(Owner owner, Pet pet)
    {
        pet.OwnerId = owner.Id;
        var query = new GetWeatherForPetQuery { PetId = pet.Id, Sub = owner.Sub };
        _ownerRepository.Setup(x => x.GetOwnerAsync(owner.Sub, It.IsAny<CancellationToken>())).ReturnsAsync(owner);
        _petRepository.Setup(x => x.GetPetAsync(query.PetId, It.IsAny<CancellationToken>())).ReturnsAsync(pet);
        _weatherService.Setup(x => x.GetWeatherDataAsync(pet.Location, It.IsAny<CancellationToken>())).ReturnsAsync(new WeatherData());
        _petStatusService.Setup(x => x.GetPetStatuses(pet.Species, It.IsAny<WeatherData>())).Returns(Errors.Pet.UnknownSpecies);

        var result = await _sut.Handle(query, CancellationToken.None);

        Assert.True(result.IsError);
        Assert.Equal(Errors.Pet.UnknownSpecies.Code, result.FirstError.Code);
    }

    [Theory, AutoData]
    public async Task Success(Owner owner, Pet pet, WeatherData weatherData)
    {
        pet.OwnerId = owner.Id;
        var query = new GetWeatherForPetQuery { PetId = pet.Id, Sub = owner.Sub };
        _ownerRepository.Setup(x => x.GetOwnerAsync(owner.Sub, It.IsAny<CancellationToken>())).ReturnsAsync(owner);
        _petRepository.Setup(x => x.GetPetAsync(query.PetId, It.IsAny<CancellationToken>())).ReturnsAsync(pet);
        _weatherService.Setup(x => x.GetWeatherDataAsync(pet.Location, It.IsAny<CancellationToken>())).ReturnsAsync(weatherData);
        _petStatusService.Setup(x => x.GetPetStatuses(pet.Species, It.IsAny<WeatherData>())).Returns(new List<PetStatus>() { PetStatus.Content });

        var result = await _sut.Handle(query, CancellationToken.None);

        Assert.False(result.IsError);
        Assert.NotNull(result.Value);
        Assert.Equal(pet, result.Value.Pet);
        Assert.Equal(weatherData, result.Value.WeatherData);
        Assert.Contains(PetStatus.Content, result.Value.Statuses);
    }
}