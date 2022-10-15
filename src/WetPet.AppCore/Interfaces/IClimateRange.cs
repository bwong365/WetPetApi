using WetPet.AppCore.Entities;

namespace WetPet.AppCore.Interfaces;

public interface IClimateRange
{
    public int TempMinC { get; }
    public int TempMaxC { get; }
    public bool IsAquatic { get; }
}