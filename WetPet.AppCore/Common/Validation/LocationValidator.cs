using FluentValidation;
using WetPet.AppCore.ValueObjects;

namespace WetPet.AppCore.Common.Validation;

public class LocationValidator : AbstractValidator<Location>
{
    public LocationValidator()
    {
        RuleFor(x => x.City).NotEmpty().WithMessage("City cannot be empty");
        RuleFor(x => x.State).NotEmpty().WithMessage("State cannot be empty");
        RuleFor(x => x.Country).NotEmpty().WithMessage("Country cannot be empty");
    }
}