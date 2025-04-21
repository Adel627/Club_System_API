namespace Club_System_API.Dtos.Users;

public record CreateUserRequest(
    string FirstName,
    string LastName,
    DateOnly Birth_Of_Date ,
    DateOnly? Renewal_date,
    IFormFile? Image ,
    string PhoneNumber,
    string Password
);