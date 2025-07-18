using FluentValidation;

namespace Club_System_API.Dtos.Service
{
    public class ImageRequestValidator:AbstractValidator<ImageRequest>
    {
        public ImageRequestValidator()
        {
            RuleFor(x=> x.Url).NotNull().NotEmpty(); 
        }
    }
}
