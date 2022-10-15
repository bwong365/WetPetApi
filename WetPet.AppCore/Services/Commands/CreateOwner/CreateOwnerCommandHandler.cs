using ErrorOr;
using MediatR;
using WetPet.AppCore.Common.Errors;
using WetPet.AppCore.Entities;
using WetPet.AppCore.Interfaces;

namespace WetPet.AppCore.Services.Commands.CreateOwner;

public class CreateOwnerCommandHandler : IRequestHandler<CreateOwnerCommand, ErrorOr<Unit>>
{
    private readonly IOwnerRepository _repository;

    public CreateOwnerCommandHandler(IOwnerRepository repository)
    {
        _repository = repository;
    }

    public async Task<ErrorOr<Unit>> Handle(CreateOwnerCommand command, CancellationToken ct)
    {
        if (await _repository.GetOwnerAsync(command.Sub) is not null)
        {
            return Errors.Owner.DuplicateSub;
        }

        var owner = new Owner
        {
            Sub = command.Sub
        };

        await _repository.CreateOwnerAsync(owner, ct);
        return Unit.Value;
    }
}
