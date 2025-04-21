namespace Club_System_API.Dtos.PhoneVerification
{
    public record VerifyRequest(
         string PhoneNumber ,
           string Code 
        );
}
