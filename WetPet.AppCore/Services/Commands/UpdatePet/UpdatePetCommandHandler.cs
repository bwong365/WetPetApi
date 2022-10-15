using ErrorOr;
using MediatR;
using WetPet.AppCore.Common.Errors;
using WetPet.AppCore.Entities;
using WetPet.AppCore.Interfaces;

namespace WetPet.AppCore.Services.Commands.UpdatePet;

public class UpdatePetCommandHandler : IRequestHandler<UpdatePetCommand, ErrorOr<Pet>>
{
    private readonly IOwnerRepository _ownerRepository;
    private readonly IPetRepository _petRepository;
    private readonly ILocationService _locationService;

    public UpdatePetCommandHandler(IOwnerRepository ownerRepository, IPetRepository petRepository, ILocationService locationService)
    {
        _ownerRepository = ownerRepository;
        _petRepository = petRepository;
        _locationService = locationService;
    }

    public async Task<ErrorOr<Pet>> Handle(UpdatePetCommand request, CancellationToken ct)
    {
        var owner = await _ownerRepository.GetOwnerAsync(request.Sub, ct);
        if (owner is null)
        {
            return Errors.Owner.NotFound;
        }

        if (request.Location is not null)
        {
            var coordinates = await _locationService.GetCoordinatesAsync(request.Location);
            if (coordinates.IsError)
            {
                return coordinates.Errors;
            }
        }

        var pet = await _petRepository.GetPetAsync(request.PetId, ct);
        if (pet is null || pet.OwnerId != owner.Id)
        {
            return Errors.Pet.NotFound;
        }

        if (request.Name is not null)
        {
            pet.Name = request.Name;
        }

        if (request.Location is not null)
        {
            pet.Location = request.Location;
        }

        return await _petRepository.UpdatePetAsync(pet, ct);
    }
}