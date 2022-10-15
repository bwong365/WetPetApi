using ErrorOr;
using MediatR;
using WetPet.AppCore.Entities;

namespace WetPet.AppCore.Services.Queries.GetPets;

public record GetPetsQuery : IRequest<ErrorOr<List<Pet>>>
{
    public string Sub { get; init; } = null!;
}