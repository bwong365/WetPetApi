using ErrorOr;
using MediatR;
using WetPet.AppCore.Aggregates;
using WetPet.AppCore.Common.Errors;
using WetPet.AppCore.Interfaces;

namespace WetPet.AppCore.Services.Queries.GetWeatherForPet;

public class GetWeatherForPetQueryHandler : IRequestHandler<GetWeatherForPetQuery, ErrorOr<PetReport>>
{
    private readonly IPetRepository _petRepository;
    private readonly IOwnerRepository _ownerRepository;
    private readonly IWeatherService _weatherService;
    private readonly IPetStatusService _petStatusService;

    public GetWeatherForPetQueryHandler(IPetRepository petRepository, IOwnerRepository ownerRepository, IWeatherService weatherService, IPetStatusService petStatusService)
    {
        _petRepository = petRepository;
        _ownerRepository = ownerRepository;
        _petStatusService = petStatusService;
        _weatherService = weatherService;
    }


    public async Task<ErrorOr<PetReport>> Handle(GetWeatherForPetQuery query, CancellationToken ct)
    {
        var owner = await _ownerRepository.GetOwnerAsync(query.Sub, ct);
        if (owner is null)
        {
            return Errors.Owner.NotFound;
        }

        var pet = await _petRepository.GetPetAsync(query.PetId, ct);
        if (pet is null || pet.OwnerId != owner.Id)
        {
            return Errors.Pet.NotFound;
        }

        var weatherData = await _weatherService.GetWeatherDataAsync(pet.Location, ct);
        if (weatherData.IsError)
        {
            return weatherData.Errors;
        }

        var statuses = _petStatusService.GetPetStatuses(pet.Species, weatherData.Value);
        if (statuses.IsError)
        {
            return statuses.Errors;
        }

        return new PetReport
        {
            Pet = pet,
            WeatherData = weatherData.Value,
            Statuses = statuses.Value
        };
    }
}