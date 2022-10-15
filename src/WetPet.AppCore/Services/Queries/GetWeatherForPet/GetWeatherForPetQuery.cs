using ErrorOr;
using MediatR;
using WetPet.AppCore.Aggregates;

namespace WetPet.AppCore.Services.Queries.GetWeatherForPet;

public record GetWeatherForPetQuery : IRequest<ErrorOr<PetReport>>
{
    public Guid PetId { get; init; }
    public string Sub { get; set; } = null!;
}