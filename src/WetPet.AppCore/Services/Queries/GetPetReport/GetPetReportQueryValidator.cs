using FluentValidation;

namespace WetPet.AppCore.Services.Queries.GetPetReport;

public class GetPetReportQueryValidator : AbstractValidator<GetPetReportQuery>
{
    public GetPetReportQueryValidator()
    {
        RuleFor(x => x.PetId).NotEmpty().WithMessage("PetId cannot be empty");
        RuleFor(x => x.Sub).NotEmpty().WithMessage("Sub cannot be empty");
    }
}