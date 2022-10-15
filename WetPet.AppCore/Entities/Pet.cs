
using WetPet.AppCore.Common.Enums;
using WetPet.AppCore.ValueObjects;

namespace WetPet.AppCore.Entities;

public class Pet
{
    public Guid Id { get; set; } = Guid.NewGuid();
    public string Name { get; set; } = null!;
    public Location Location { get; set; } = null!;
    public PetSpecies Species { get; set; }
    public Guid OwnerId { get; set; }

    public void UpdateOwner(Owner owner)
    {
        OwnerId = owner.Id;
    }
}