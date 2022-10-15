using FluentValidation;

namespace WetPet.AppCore.Services.Commands.CreateOwner;

public class CreateOwnerCommandValidator : AbstractValidator<CreateOwnerCommand>
{
    public CreateOwnerCommandValidator()
    {
        RuleFor(x => x.Sub).NotEmpty().WithMessage("Sub cannot be empty");
    }
}