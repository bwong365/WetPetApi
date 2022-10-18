namespace WetPet.Contracts.Pets;

public class PetReportResponse
{
    public Guid Id { get; set; }
    public string Name { get; set; } = null!;
    public string City { get; set; } = null!;
    public string State { get; set; } = null!;
    public string Country { get; set; } = null!;
    public PetSpecies Species { get; set; }

    public int TempC { get; set; }
    public List<PetStatus> Statuses { get; set; } = new();
}