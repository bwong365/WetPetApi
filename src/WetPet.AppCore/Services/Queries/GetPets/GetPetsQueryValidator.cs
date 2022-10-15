using FluentValidation;

namespace WetPet.AppCore.Services.Queries.GetPets;

public class GetPetsQueryValidator : AbstractValidator<GetPetsQuery>
{
    public GetPetsQueryValidator()
    {
        RuleFor(x => x.Sub).NotEmpty().WithMessage("Sub cannot be empty");
    }
}