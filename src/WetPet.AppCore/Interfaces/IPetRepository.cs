using WetPet.AppCore.Entities;

namespace WetPet.AppCore.Interfaces;

public interface IPetRepository
{
    Task<List<Pet>> GetPetsAsync(Guid ownerId, CancellationToken? cancellationToken = default);
    Task<Pet?> GetPetAsync(Guid petId, CancellationToken? cancellationToken = default);
    Task<Pet> AddPetAsync(Pet pet, CancellationToken? cancellationToken = default);
    Task<Pet> UpdatePetAsync(Pet pet, CancellationToken? cancellationToken = default);
    Task RemovePetAsync(Guid petId, CancellationToken? cancellationToken = default);
}