using Club_System_API.Abstractions;

namespace Club_System_API.Errors
{
    public static class ServiceCoachErrors
    {
        public static readonly Error CoachorServiceNotFound =
        new("Coach or Service.NotFound", "No Service or Coach was found with the given ID", StatusCodes.Status404NotFound);
        public static readonly Error AlreadyAssigned =
         new("ServiceCoach.DuplicatedAssign", "Coach is already assigned to this service." , StatusCodes.Status409Conflict);
        public static readonly Error CoachNotAssigned =
            new("ServiceCoach.DuplicatedAssign", "Coach not assigned to this service.", StatusCodes.Status409Conflict);


    }
}
