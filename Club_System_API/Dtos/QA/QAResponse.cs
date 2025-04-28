namespace Club_System_API.Dtos.QA
{
    public record QAResponse(
     int Id,
     string Question ,
     string Answer ,
     int SortNum ,
     string UserId 
        );
}
