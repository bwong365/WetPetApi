using FluentValidation;
using WetPet.AppCore.Common.Validation;

namespace WetPet.AppCore.Services.Commands.AddPet;

public class AddPetCommandValidator : AbstractValidator<AddPetCommand>
{
    public AddPetCommandValidator()
    {
        RuleFor(x => x.Sub).NotEmpty().WithMessage("Sub cannot be empty");
        RuleFor(x => x.Pet).NotEmpty().WithMessage("Pet must exist").SetValidator(new PetValidator());
    }
}