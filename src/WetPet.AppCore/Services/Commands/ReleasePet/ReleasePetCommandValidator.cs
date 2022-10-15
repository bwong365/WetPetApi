using FluentValidation;

namespace WetPet.AppCore.Services.Commands.ReleasePet;

public class ReleasePetCommandValidator : AbstractValidator<ReleasePetCommand>
{
    public ReleasePetCommandValidator()
    {
        RuleFor(x => x.Sub).NotEmpty().WithMessage("Sub cannot be empty");
        RuleFor(x => x.PetId).NotEmpty().WithMessage("PetId cannot be empty");
    }
}