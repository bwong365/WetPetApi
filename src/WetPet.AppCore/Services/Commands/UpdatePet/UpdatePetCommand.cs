using ErrorOr;
using MediatR;
using WetPet.AppCore.Entities;
using WetPet.AppCore.ValueObjects;

namespace WetPet.AppCore.Services.Commands.UpdatePet;

public record UpdatePetCommand : IRequest<ErrorOr<Pet>>
{
    public string Sub { get; init; } = null!;
    public Guid PetId { get; init; }
    public string? Name { get; init; }
    public Location? Location { get; init; }
}