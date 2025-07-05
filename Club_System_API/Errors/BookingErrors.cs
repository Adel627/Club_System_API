using Club_System_API.Abstractions;

  namespace Club_System_API.Errors
  {
        public static class BookingErrors
        {
            public static readonly Error BookingNotFound =
                new("Booking.NotFound", "The booking was not found.", StatusCodes.Status404NotFound);

        public static readonly Error BookingPayedBefore =
               new("Booking.PayedBefore", "The booking was Payed.", StatusCodes.Status409Conflict);


        public static readonly Error AppointmentNotFound =
                new("Booking.AppointmentNotFound", "The selected appointment does not exist.", StatusCodes.Status404NotFound);

            public static readonly Error AppointmentFull =
                new("Booking.AppointmentFull", "This appointment has reached the maximum number of attendees.", StatusCodes.Status400BadRequest);

            public static readonly Error InvalidAppointmentTime =
                new("Booking.InvalidAppointmentTime", "The appointment time is invalid or already passed.", StatusCodes.Status400BadRequest);

            public static readonly Error DuplicateBooking =
                new("Booking.DuplicateBooking", "You already have a booking for this service/appointment.", StatusCodes.Status409Conflict);

            public static readonly Error UnauthorizedBookingAccess =
                new("Booking.UnauthorizedAccess", "You are not authorized to access this booking.", StatusCodes.Status403Forbidden);

            public static readonly Error CannotCancelBooking =
                new("Booking.CannotCancel", "This booking cannot be cancelled.", StatusCodes.Status400BadRequest);

            public static readonly Error PaymentRequired =
                new("Booking.PaymentRequired", "Payment is required to complete the booking.", StatusCodes.Status402PaymentRequired);

            public static readonly Error ServiceNotAvailable =
                new("Booking.ServiceNotAvailable", "The selected service is not available for booking.", StatusCodes.Status400BadRequest);

            public static readonly Error CoachUnavailable =
                new("Booking.CoachUnavailable", "The selected coach is unavailable at the chosen time.", StatusCodes.Status400BadRequest);
        }
  }
