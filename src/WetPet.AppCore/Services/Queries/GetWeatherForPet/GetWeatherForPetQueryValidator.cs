using FluentValidation;

namespace WetPet.AppCore.Services.Queries.GetWeatherForPet;

public class GetWeatherForPetQueryValidator : AbstractValidator<GetWeatherForPetQuery>
{
    public GetWeatherForPetQueryValidator()
    {
        RuleFor(x => x.PetId).NotEmpty().WithMessage("PetId cannot be empty");
        RuleFor(x => x.Sub).NotEmpty().WithMessage("Sub cannot be empty");
    }
}