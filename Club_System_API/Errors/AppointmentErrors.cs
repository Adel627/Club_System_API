using Club_System_API.Abstractions;

namespace Club_System_API.Errors
{
    public  static class AppointmentErrors
    {
        public static readonly Error CoachNotAssigned =
            new("ServiceCoach.NotAssign", "Coach not assigned to this service You must Assign this Coach to this Service Firstly.", StatusCodes.Status409Conflict);
    }
}
