using System.Linq;
using AutoMapper;
using vega.Controllers.Resources;
using vega.Controllers.Resources.Contact;
using vega.Core.Models;

namespace vega.Mapping.MappingProfiles
{
    public class ContactMapping : Profile
    {
        public ContactMapping()
        {  
            CreateMap<ContactResource, Contact>();
            CreateMap<Contact, ContactResource>()
                    .ForMember(psr => psr.FullName,                    
                        opt => opt.MapFrom(ps =>  ps.FirstName + " " + ps.LastName));
        }

    }
}