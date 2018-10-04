using AutoMapper;
using vega.Core.Models;
using vegaplanner.Controllers.Resources;

namespace vegaplanner.Mapping.MappingProfiles
{
    public class AddressMapping : Profile
    {
        public AddressMapping()
        {  
            CreateMap<AddressResource, Address>();
            CreateMap<Address, AddressResource>(); 
        }
    }
}