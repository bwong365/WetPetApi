using ErrorOr;
using WetPet.AppCore.ValueObjects;

namespace WetPet.AppCore.Interfaces;

public interface ILocationService
{
    public Task<ErrorOr<Coordinates?>> GetCoordinatesAsync(Location location, CancellationToken? cancellationToken = default);
}