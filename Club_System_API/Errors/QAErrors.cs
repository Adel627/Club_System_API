using Club_System_API.Abstractions;

namespace Club_System_API.Errors
{
    public static class QAErrors
    {
        public static readonly Error DuplicatedSortNum =
         new("QA.DuplicatedSortNum", "Another QA with the same SortNum is already exists", StatusCodes.Status409Conflict);

        public static readonly Error QANotFound =
        new("QA.NotFound", "No QA was found with the given ID", StatusCodes.Status404NotFound);

    }
}
