using ErrorOr;

namespace WetPet.AppCore.Common.Errors;

public static partial class Errors
{
    public static class Location
    {
        public static Error InvalidLocation = Error.Validation(code: "Location.Invalid", description: "The location is invalid");
        public static Error Failure = Error.Failure(code: "Location.Failure", description: "Failed to get the location");
    }
}