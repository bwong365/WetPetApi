using Microsoft.EntityFrameworkCore;
using WetPet.AppCore.Entities;
using WetPet.AppCore.Interfaces;

namespace WetPet.Infrastructure.Persistence;

public class OwnerRepository : IOwnerRepository
{
    private readonly AppDbContext _context;

    public OwnerRepository(AppDbContext context)
    {
        _context = context;
    }

    public async Task CreateOwnerAsync(Owner owner, CancellationToken? ct)
    {
        _context.Owners.Add(owner);
        await _context.SaveChangesAsync(ct ?? default);
    }

    public async Task<Owner?> GetOwnerAsync(string sub, CancellationToken? ct)
    {
        return await _context.Owners.FirstOrDefaultAsync(o => o.Sub == sub, ct ?? default);
    }
}