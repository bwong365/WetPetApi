using ErrorOr;
using MediatR;

namespace WetPet.AppCore.Services.Commands.CreateOwner;

public record CreateOwnerCommand : IRequest<ErrorOr<Unit>>
{
    public string Sub { get; init; } = null!;
}