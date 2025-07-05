using Club_System_API.Dtos.Users;

namespace Club_System_API.Dtos.Service
{
    public record ServiceResponse(
    
         int Id ,
         string Name ,
         decimal Price, 
        string Description,
        double AverageRating,
        string? ContentType,
        string? Base64Data
    );
}
