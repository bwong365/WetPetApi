using ErrorOr;
using MediatR;
using WetPet.AppCore.Common.Errors;
using WetPet.AppCore.Entities;
using WetPet.AppCore.Interfaces;

namespace WetPet.AppCore.Services.Queries.GetPets;

public class GetPetsQueryHandler : IRequestHandler<GetPetsQuery, ErrorOr<List<Pet>>>
{
    private readonly IPetRepository _repository;
    private readonly IOwnerRepository _ownerRepository;

    public GetPetsQueryHandler(IPetRepository repository, IOwnerRepository ownerRepository)
    {
        _repository = repository;
        _ownerRepository = ownerRepository;
    }

    public async Task<ErrorOr<List<Pet>>> Handle(GetPetsQuery request, CancellationToken ct)
    {
        var owner = await _ownerRepository.GetOwnerAsync(request.Sub, ct);
        if (owner is null)
        {
            return Errors.Owner.NotFound;
        }
        return await _repository.GetPetsAsync(owner.Id, ct);
    }
}