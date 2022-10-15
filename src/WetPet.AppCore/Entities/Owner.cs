namespace WetPet.AppCore.Entities;

public class Owner
{
    public Guid Id { get; set; } = Guid.NewGuid();
    public string Sub { get; set; } = null!;
    public List<Pet> Pets { get; set; } = new();
}