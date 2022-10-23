namespace WetPet.Contracts.Pets;

public class PetAdditionRequest
{
    public string Name { get; set; } = null!;
    public string City { get; set; } = null!;
    public string? State { get; set; }
    public string Country { get; set; } = null!;
    public PetSpecies Species { get; set; }
}