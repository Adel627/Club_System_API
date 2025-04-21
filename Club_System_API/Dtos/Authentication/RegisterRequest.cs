namespace Club_System_API.Dtos.Authentication;


public record RegisterRequest(
      string FirstName,
      string LastName,
      DateOnly Birth_Of_Date,
      IFormFile? Image,
      string PhoneNumber,
      string Password
  
);