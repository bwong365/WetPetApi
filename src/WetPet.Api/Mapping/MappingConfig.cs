using Mapster;
using WetPet.AppCore.Aggregates;
using WetPet.AppCore.Entities;
using WetPet.AppCore.Services.Commands.AddPet;
using WetPet.AppCore.Services.Commands.UpdatePet;
using WetPet.AppCore.ValueObjects;
using WetPet.Contracts.Pets;

namespace WetPet.Api.Mapping;

public class MappingConfig : IRegister
{
    public void Register(TypeAdapterConfig config)
    {
        config.NewConfig<PetAdditionRequest, Location>();

        config.NewConfig<PetAdditionRequest, Pet>()
            .Map(dest => dest.Location, src => src)
            .Ignore(dest => dest.Id)
            .Ignore(dest => dest.OwnerId)
            .Ignore(dest => dest.CreatedDateUtc);

        config.NewConfig<(string sub, PetAdditionRequest request), AddPetCommand>()
            .Map(dest => dest.Sub, src => src.sub)
            .Map(dest => dest.Pet, src => src.request);

        config.NewConfig<(string sub, Guid petId, PetUpdateRequest request), UpdatePetCommand>()
            .Map(dest => dest.Sub, src => src.sub)
            .Map(dest => dest.PetId, src => src.petId)
            .Map(dest => dest.Name, src => src.request.Name)
            .Map(dest => dest.Location, src => src.request, src => new[] { src.request.City, src.request.State, src.request.Country }.Any(x => x != null));

        config.NewConfig<Pet, PetResponse>()
            .Map(dest => dest, src => src.Location);

        config.NewConfig<PetReport, PetReportResponse>()
            .Map(dest => dest, src => src.Pet)
            .Map(dest => dest, src => src.WeatherData)
            .Map(dest => dest, src => src.Pet.Location);

    }
}