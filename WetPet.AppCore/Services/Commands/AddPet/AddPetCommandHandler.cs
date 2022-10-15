using ErrorOr;
using MediatR;
using WetPet.AppCore.Common.Errors;
using WetPet.AppCore.Entities;
using WetPet.AppCore.Interfaces;

namespace WetPet.AppCore.Services.Commands.AddPet;

public class AddPetCommandHandler : IRequestHandler<AddPetCommand, ErrorOr<Pet>>
{
    private readonly IOwnerRepository _repository;
    private readonly IPetRepository _petRepository;
    private readonly ILocationService _locationService;

    public AddPetCommandHandler(IOwnerRepository repository, IPetRepository petRepository, ILocationService locationService)
    {
        _repository = repository;
        _petRepository = petRepository;
        _locationService = locationService;
    }

    public async Task<ErrorOr<Pet>> Handle(AddPetCommand request, CancellationToken ct)
    {
        var owner = await _repository.GetOwnerAsync(request.Sub, ct);
        if (owner is null)
        {
            return Errors.Owner.NotFound;
        }

        var location = await _locationService.GetCoordinatesAsync(request.Pet.Location, ct);
        if (location.IsError)
        {
            return location.Errors;
        }

        request.Pet.UpdateOwner(owner);
        return await _petRepository.AddPetAsync(request.Pet, ct);
    }
}