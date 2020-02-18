using AutoMapper;
using Shopping.Core.Domains;
using Shopping.Core.Dtos.User;

namespace Shopping.Core.Mappings
{
    public class UserProfile : Profile
    {
        public UserProfile()
        {
            CreateMap<ApplicationUser, UserDetailInfoDto>()
                .ForAllOtherMembers(x => x.UseDestinationValue());
        }
    }
}