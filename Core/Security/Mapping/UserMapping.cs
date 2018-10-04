using AutoMapper;
using vegaplanner.Core.Models.Security;
using vegaplanner.Core.Models.Security.Resources;

namespace vegaplanner.Core.Models.Security.Mapping
{
    public class UserMapping : Profile
    {
        public UserMapping()
        {
            CreateMap<RegistrationResource, AppUser>().ForMember(au => au.UserName, map => map.MapFrom(vm => vm.Email));
        }
    }
}