using ErrorOr;
using MediatR;

namespace WetPet.AppCore.Services.Commands.ReleasePet;

public record ReleasePetCommand : IRequest<ErrorOr<Unit>>
{
    public string Sub { get; init; } = null!;
    public Guid PetId { get; init; }
}