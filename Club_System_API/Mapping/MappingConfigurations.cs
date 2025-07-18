using Club_System_API.Models;
using Mapster;
using Club_System_API.Dtos.Authentication;
using Club_System_API.Dtos.Users;
using System.IO;
using Club_System_API.Helper;
using Club_System_API.Dtos.Service;
using Club_System_API.Dtos.Coaches;
using Club_System_API.Dtos.Membership;
using Club_System_API.Dtos.CoachReview;


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
                .Map(dest => dest.Roles, src => src.roles)
                .Map(dest=> dest.MembershipName ,
                         src => src.user.UserMemberships.Select(x=>x.Membership.Name));

            config.NewConfig<ApplicationUser, UserProfileResponse>()
                .Map(dest => dest.ContentType, src => src.ImageContentType)
               .Map(dest => dest.Base64Data,
               src => src.Image != null ?
                $"data:{src.ImageContentType};base64,{Convert.ToBase64String(src.Image)}" : null)
               .Map(dest => dest.MembershipName ,
               src=>src.UserMemberships.Select(x=>x.Membership.Name).FirstOrDefault())
               
               .Map(dest=> dest.MembershipStartDate,
               src=> src.UserMemberships.Select(x=> x.StartDate).FirstOrDefault())
                      
               .Map(dest => dest.MembershipEndDate,
               src => src.UserMemberships.Select(x => x.EndDate).FirstOrDefault())

               .Map(dest => dest.Services,
                    src => src.Bookings
               .Where(b => b.Appointment != null && b.Appointment.Service != null)
               .Select(b => b.Appointment.Service.Name ?? string.Empty)
               .ToList())

               ;



            config.NewConfig<Coach, CoachWithReviewsResponse>()
               .Map(dest => dest.Achievments,
                    src => src.achievments.Select(a => a.Name).ToList())

                .Map(dest => dest.ContentType, src => src.ImageContentType)

               .Map(dest => dest.Base64Data,
                    src => src.Image != null ? Convert.ToBase64String(src.Image) : null)

               .Map(dest => dest.ReviewCoachResponse,
                    src => src.Rating.Select(r => new CoachReviewWithUserImageResponse(
                           r.User.ImageContentType,
                           r.User.Image != null ? Convert.ToBase64String(r.User.Image) : null,
                           r.User.FirstName,
                           r.User.LastName,
                           r.ReviewAt,
                           r.Rating,
                           r.Review      
                          )).ToList());

        }
    }
}
