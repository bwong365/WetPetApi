using WetPet.AppCore.Entities;

namespace WetPet.AppCore.Interfaces;

public interface IOwnerRepository
{
    public Task CreateOwnerAsync(Owner owner, CancellationToken? cancellationToken = default);
    public Task<Owner?> GetOwnerAsync(string sub, CancellationToken? cancellationToken = default);
}