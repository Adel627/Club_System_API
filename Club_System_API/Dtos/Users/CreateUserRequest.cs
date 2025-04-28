namespace Club_System_API.Dtos.Users;

public record CreateUserRequest(
    string FirstName,
    string LastName,
    string PhoneNumber,
    DateOnly Birth_Of_Date ,
    IFormFile? Image ,
    string Password,
    bool IsMember
);