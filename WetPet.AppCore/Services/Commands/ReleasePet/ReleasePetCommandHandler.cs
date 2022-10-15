using MediatR;
using ErrorOr;
using WetPet.AppCore.Common.Errors;
using WetPet.AppCore.Interfaces;

namespace WetPet.AppCore.Services.Commands.ReleasePet;

public class ReleasePetCommandHandler : IRequestHandler<ReleasePetCommand, ErrorOr<Unit>>
{

    private readonly IOwnerRepository _ownerRepository;
    private readonly IPetRepository _petRepository;

    public ReleasePetCommandHandler(IOwnerRepository ownerRepository, IPetRepository petRepository)
    {
        _ownerRepository = ownerRepository;
        _petRepository = petRepository;
    }

    public async Task<ErrorOr<Unit>> Handle(ReleasePetCommand request, CancellationToken ct)
    {
        var owner = await _ownerRepository.GetOwnerAsync(request.Sub, ct);
        if (owner is null)
        {
            return Errors.Owner.NotFound;
        }

        var pet = await _petRepository.GetPetAsync(request.PetId, ct);
        if (pet is null || pet.OwnerId != owner.Id)
        {
            return Errors.Pet.NotFound;
        }

        await _petRepository.RemovePetAsync(pet.Id, ct);
        return Unit.Value;
    }
}