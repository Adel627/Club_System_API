
namespace Club_System_API.Dtos.Coaches
{
    public record CoachRequest
    (
         string FirstName ,
         string LastName,
         string Specialty, 
         string Bio,
         DateOnly Birth_Of_Date ,
         string PhoneNumber,
         int Salary ,
         IFormFile? Image 
    );
}
 