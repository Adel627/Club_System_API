using Club_System_API.Abstractions;

namespace Club_System_API.Errors
{
    public static class ServiceErrors
    {
        public static readonly Error ServiceNotFound =
        new("Service.NotFound", "No Service was found with the given ID", StatusCodes.Status404NotFound);

        public static readonly Error DuplicatedImage =
        new("Service.DuplicatedImage", "This image is already exists", StatusCodes.Status409Conflict);


    }
}
