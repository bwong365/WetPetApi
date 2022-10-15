using System.Text.Json;
using WetPet.Infrastructure.Common.Serialization;

namespace WetPet.Infrastructure.Test.Common.Serialization;

public class SnakeCaseNamingPolicyTests
{
    private class TestModel
    {
        public string? NumberOne { get; set; }
        public string? NumberTwo { get; set; }
        public string? Three { get; set; }
    }
    [Fact]
    public void DeserializesProperly()
    {
        var json = @"{
            ""number_one"": ""one"",
            ""number_two"": ""two"",
            ""three"": ""three""
        }";

        var result = JsonSerializer.Deserialize<TestModel>(json, new JsonSerializerOptions
        {
            PropertyNamingPolicy = new SnakeCaseNamingPolicy(),
        });

        Assert.Equal("one", result?.NumberOne);
        Assert.Equal("two", result?.NumberTwo);
        Assert.Equal("three", result?.Three);
    }
}