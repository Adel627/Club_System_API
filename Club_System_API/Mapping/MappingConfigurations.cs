using Club_System_API.Models;
using Mapster;
using Club_System_API.Dtos.Authentication;
using Club_System_API.Dtos.Users;
using System.IO;
using Club_System_API.Helper;
using Club_System_API.Dtos.Service;
using Club_System_API.Dtos.Coaches;
using Club_System_API.Dtos.Membership;


namespace Club_System_API.Mapping
{
    public class MappingConfigurations:IRegister
    {
        public void Register(TypeAdapterConfig config)
        {
           

    config.NewConfig<IFormFile, byte[]>()
          .MapWith(file => FormFileExtensions.ConvertToBytes(file));

            config.NewConfig<RegisterRequest, ApplicationUser>()
                .Map(dest => dest.Image, src => src.Image)
                .Map(dest => dest.ImageContentType,
                 src => src.Image != null ? src.Image.ContentType : null);

            config.NewConfig<CreateUserRequest, ApplicationUser>()
                .Map(dest => dest.Image, src => src.Image)
                .Map(dest => dest.ImageContentType,
                src => src.Image != null?src.Image.ContentType:null);


            config.NewConfig<MembershipRequest, Membership>()
                 .Map(dest => dest.Image, src => src.Image)
                .Map(dest => dest.ImageContentType,
                src => src.Image != null ? src.Image.ContentType : null);


            config.NewConfig<Membership, MembershipResponse>()
              .Map(dest => dest.ContentType, src => src.ImageContentType)
                .Map(dest => dest.Base64Data,
                src => src.Image != null ?
                 $"data:{src.ImageContentType};base64,{Convert.ToBase64String(src.Image)}" : null);


            config.NewConfig<ApplicationUser,UserProfileResponse>()
                 .Map(dest => dest.ContentType, src => src.ImageContentType)
                .Map(dest => dest.Base64Data,
                src => src.Image != null ?
                 $"data:{src.ImageContentType};base64,{Convert.ToBase64String(src.Image)}" : null);

            config.NewConfig<ApplicationUser, AddingUserResponse>()
                .Map(dest => dest.ContentType, src => src.ImageContentType)
                .Map(dest => dest.Base64Data,
                src=>src.Image!=null?
                 $"data:{src.ImageContentType};base64,{Convert.ToBase64String(src.Image)}":null);

            config.NewConfig<ApplicationUser, UserResponse>()
                .Map(dest => dest.ContentType, src => src.ImageContentType)
                .Map(dest => dest.Base64Data,
                src => src.Image != null ?
                 $"data:{src.ImageContentType};base64,{Convert.ToBase64String(src.Image)}" : null);

            config.NewConfig<ServiceRequest, Service>()
              .Map(dest => dest.Image, src => src.Image)
                .Map(dest => dest.ImageContentType,
                src => src.Image != null ? src.Image.ContentType : null);


            config.NewConfig<Service, ServiceResponse>()
            .Map(dest => dest.ContentType, src => src.ImageContentType)
                .Map(dest => dest.Base64Data,
                src => src.Image != null ?
                 $"data:{src.ImageContentType};base64,{Convert.ToBase64String(src.Image)}" : null);


            config.NewConfig<CoachRequest,Coach>()
       .Map(dest => dest.Image, src => src.Image)
                .Map(dest => dest.ImageContentType,
                src => src.Image != null ? src.Image.ContentType : null);

            config.NewConfig<Coach, CoachResponse>()
                .Map(dest => dest.ContentType, src => src.ImageContentType)
                .Map(dest => dest.Base64Data,
                src => src.Image != null ?
                 $"data:{src.ImageContentType};base64,{Convert.ToBase64String(src.Image)}" : null);

            config.NewConfig<(ApplicationUser user, IList<string> roles), UserResponse>()
                .Map(dest => dest.ContentType, src => src.user. ImageContentType)
         .Map(dest => dest.Base64Data,
            src => src.user.Image !=null?
            $"data:{src.user. ImageContentType};base64,{Convert.ToBase64String(src.user. Image)}" : null)
                .Map(dest => dest, src => src.user)
                .Map(dest => dest.Roles, src => src.roles);



        }
    }
}
