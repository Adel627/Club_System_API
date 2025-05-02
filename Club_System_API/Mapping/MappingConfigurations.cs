using Club_System_API.Models;
using Mapster;
using Club_System_API.Dtos.Authentication;
using Club_System_API.Dtos.Users;
using System.IO;
using Club_System_API.Helper;
using Club_System_API.Dtos.Service;
using Club_System_API.Dtos.Coaches;


namespace Club_System_API.Mapping
{
    public class MappingConfigurations:IRegister
    {
        public void Register(TypeAdapterConfig config)
        {


            config.NewConfig<IFormFile, byte[]>()
          .MapWith(file => FormFileExtensions.ConvertToBytes(file)); 

            config.NewConfig<RegisterRequest, ApplicationUser>()
                .Map(dest => dest.Image, src => src.Image);

            config.NewConfig<CreateUserRequest, ApplicationUser>()
                .Map(dest => dest.Image, src => src.Image);

            config.NewConfig<ApplicationUser,UserProfileResponse>()
                .Map(dest => dest.Image, src => src.Image);

            config.NewConfig<ServiceRequest, Service>()
              .Map(dest => dest.Image, src => src.Image);


            config.NewConfig<CoachRequest,Coach>()
              .Map(dest => dest.Image, src => src.Image);






            config.NewConfig<(ApplicationUser user, IList<string> roles), UserResponse>()
                .Map(dest => dest, src => src.user)
                .Map(dest => dest.Roles, src => src.roles);

        }
    }
}
