using ErrorOr;
using MediatR;
using WetPet.AppCore.Entities;

namespace WetPet.AppCore.Services.Commands.AddPet;

public record AddPetCommand : IRequest<ErrorOr<Pet>>
{
    public Pet Pet { get; init; } = null!;
    public string Sub { get; init; } = null!;
}