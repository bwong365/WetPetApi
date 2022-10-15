using ErrorOr;

namespace WetPet.AppCore.Common.Errors;

public static partial class Errors
{
    public static class Pet
    {
        public static Error UnknownSpecies = Error.Unexpected(code: "Pet.UnknownSpecies", description: "This species requires more research");
        public static Error NotFound = Error.NotFound(code: "Pet.NotFound", description: "The pet could not be found!");
    }
}