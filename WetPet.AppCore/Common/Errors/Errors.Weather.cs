using ErrorOr;

namespace WetPet.AppCore.Common.Errors;

public static partial class Errors
{
    public static class Weather
    {
        public static Error Failure = Error.Unexpected(code: "Weather.Failure", description: "Failed to get the weather");
    }
}