using ErrorOr;

namespace WetPet.AppCore.Common.Errors;

public static partial class Errors
{
    public static class Owner
    {
        public static Error NotFound = Error.NotFound(code: "Owner.NotFound", description: "The owner could not be found!");
        public static Error DuplicateSub = Error.Conflict(code: "Owner.DuplicateSub", description: "The owner already exists!");
    }
}