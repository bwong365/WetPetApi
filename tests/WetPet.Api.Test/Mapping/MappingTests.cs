using Mapster;
using WetPet.Api.Mapping;

namespace WetPet.Api.Test.Mapping;

public class MappingTests
{
    private readonly TypeAdapterConfig _sut;

    public MappingTests()
    {
        _sut = new TypeAdapterConfig();
        _sut.Apply(new MappingConfig());
        _sut.RequireDestinationMemberSource = true;
    }

    [Fact]
    public void Validate_Config()
    {
        _sut.Compile();
    }
}