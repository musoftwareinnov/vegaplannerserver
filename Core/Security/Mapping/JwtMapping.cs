using AutoMapper;
using vegaplanner.Core.Models.Security.JWT;

namespace vegaplanner.Core.Models.Security.Mapping
{
    public class JwtMapping: Profile
    {
        public JwtMapping()
        {
            CreateMap<JwtModel, JwtResource>().ForMember(jwtR => jwtR.Id, map => map.MapFrom(jwt => jwt.Id.Value));
        }
    }
}