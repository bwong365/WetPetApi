using FluentValidation;
using WetPet.AppCore.Common.Validation;

namespace WetPet.AppCore.Services.Commands.UpdatePet;

public class UpdatePetCommandValidator : AbstractValidator<UpdatePetCommand>
{
    public UpdatePetCommandValidator()
    {
        RuleFor(x => x.Sub).NotEmpty().WithMessage("Sub cannot be empty");

        RuleFor(x => x.PetId).NotEmpty().WithMessage("PetId cannot be empty");

        RuleFor(x => x.Name).MinimumLength(1).WithMessage("Name can't be empty");

        RuleFor(x => x.Location).SetValidator(new LocationValidator()!);
    }
}