using FluentValidation;
using WetPet.AppCore.Entities;

namespace WetPet.AppCore.Common.Validation;

public class PetValidator : AbstractValidator<Pet>
{
    public PetValidator()
    {
        RuleFor(x => x.Name).NotEmpty().WithMessage("Pet must have a name");
        RuleFor(x => x.Location).NotNull().WithMessage("Pet must have a location").SetValidator(new LocationValidator());
        RuleFor(x => x.Species).IsInEnum().WithMessage("Pet must have a valid species");
    }
}