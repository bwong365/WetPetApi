using System.Text.Json;
using WetPet.Infrastructure.Common.Utils;

namespace WetPet.Infrastructure.Common.Serialization;

// This will be used for deserializing snake cased responses
public class SnakeCaseNamingPolicy : JsonNamingPolicy
{
    public override string ConvertName(string name)
    {
        return name.ToSnakeCase();
    }
}
