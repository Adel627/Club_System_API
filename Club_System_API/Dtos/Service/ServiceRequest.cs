namespace Club_System_API.Dtos.Service
{
    public record CoachRequest (

         string Name,
         decimal Price,
        string Description,
        IFormFile? Image

        );
}
