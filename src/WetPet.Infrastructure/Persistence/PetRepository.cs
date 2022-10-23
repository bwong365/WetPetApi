using Microsoft.EntityFrameworkCore;
using WetPet.AppCore.Entities;
using WetPet.AppCore.Interfaces;

namespace WetPet.Infrastructure.Persistence;

public class PetRepository : IPetRepository
{
    private readonly AppDbContext _context;

    public PetRepository(AppDbContext context)
    {
        _context = context;
    }

    public async Task<Pet> AddPetAsync(Pet pet, CancellationToken? cancellationToken = null)
    {
        _context.Pets.Add(pet);
        await _context.SaveChangesAsync(cancellationToken ?? CancellationToken.None);
        return pet;
    }

    public async Task<Pet?> GetPetAsync(Guid petId, CancellationToken? cancellationToken = null)
    {
        return await _context.Pets.FirstOrDefaultAsync(p => p.Id == petId, cancellationToken ?? CancellationToken.None);
    }

    public async Task<List<Pet>> GetPetsAsync(Guid ownerId, CancellationToken? cancellationToken = null)
    {
        return await _context.Pets.Where(p => p.OwnerId == ownerId).OrderBy(p => p.CreatedDateUtc).ToListAsync(cancellationToken ?? CancellationToken.None);
    }

    public async Task RemovePetAsync(Guid petId, CancellationToken? cancellationToken = null)
    {
        var pet = await _context.Pets.FirstOrDefaultAsync(p => p.Id == petId);
        if (pet is null)
        {
            throw new KeyNotFoundException("Pet was not found");
        }
        _context.Pets.Remove(pet);
        await _context.SaveChangesAsync(cancellationToken ?? CancellationToken.None);
    }

    public async Task<Pet> UpdatePetAsync(Pet update, CancellationToken? cancellationToken = null)
    {
        var pet = _context.Pets.FirstOrDefault(p => p.Id == update.Id);
        if (pet is null)
        {
            throw new KeyNotFoundException("Pet was not found");
        }
        pet.Name = update.Name;
        pet.Location = update.Location;
        await _context.SaveChangesAsync(cancellationToken ?? CancellationToken.None);
        return pet;
    }
}