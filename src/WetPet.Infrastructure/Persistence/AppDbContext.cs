using Microsoft.EntityFrameworkCore;
using WetPet.AppCore.Entities;
using WetPet.AppCore.ValueObjects;

namespace WetPet.Infrastructure.Persistence;

public class AppDbContext : DbContext
{
    public DbSet<Pet> Pets { get; set; } = null!;
    public DbSet<Owner> Owners { get; set; } = null!;

    public AppDbContext(DbContextOptions<AppDbContext> options) : base(options) { }

    protected override void OnModelCreating(ModelBuilder modelBuilder)
    {
        base.OnModelCreating(modelBuilder);
        modelBuilder.Entity<Pet>().OwnsOne<Location>(nameof(Pet.Location));
    }
}