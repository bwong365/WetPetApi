namespace WetPet.Contracts.Pets;

public class PetUpdateRequest
{
    public string? Name { get; set; }
    public string? City { get; set; }
    public string? State { get; set; }
    public string? Country { get; set; }
}