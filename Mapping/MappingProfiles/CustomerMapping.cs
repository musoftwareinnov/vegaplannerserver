using AutoMapper;
using vega.Controllers.Resources;
using vega.Core.Models;
using vega.Extensions.DateTime;

namespace vega.Mapping.MappingProfiles
{
    public class CustomerMapping : Profile
    {
        public CustomerMapping()
        { 
            CreateMap<PlanningCustomerResource, Customer>();
            CreateMap<Customer, PlanningCustomerResource>()
                    .ForMember(psr => psr.FirstName,                    
                        opt => opt.MapFrom(ps =>  ps.CustomerContact.FirstName))
                    .ForMember(psr => psr.LastName,                    
                        opt => opt.MapFrom(ps =>  ps.CustomerContact.LastName))
                    .ForMember(psr => psr.EmailAddress,                    
                        opt => opt.MapFrom(ps =>  ps.CustomerContact.EmailAddress))
                    .ForMember(psr => psr.TelephoneHome,                    
                        opt => opt.MapFrom(ps =>  ps.CustomerContact.TelephoneHome))
                    .ForMember(psr => psr.TelephoneWork,                    
                        opt => opt.MapFrom(ps =>  ps.CustomerContact.TelephoneWork))
                    .ForMember(psr => psr.TelephoneMobile,                    
                        opt => opt.MapFrom(ps =>  ps.CustomerContact.TelephoneMobile))

                    //Address Details
                    .ForMember(psr => psr.AddressLine1,                    
                        opt => opt.MapFrom(ps =>  ps.CustomerAddress.AddressLine1))
                    .ForMember(psr => psr.AddressLine2,                    
                        opt => opt.MapFrom(ps =>  ps.CustomerAddress.AddressLine2))
                    .ForMember(psr => psr.Postcode,                    
                        opt => opt.MapFrom(ps =>  ps.CustomerAddress.Postcode));

            CreateMap<Customer, CustomerResource>()

                    //Contact Details
                    .ForMember(psr => psr.FirstName,                    
                        opt => opt.MapFrom(ps =>  ps.CustomerContact.FirstName))
                    .ForMember(psr => psr.LastName,                    
                        opt => opt.MapFrom(ps =>  ps.CustomerContact.LastName))
                    .ForMember(psr => psr.EmailAddress,                    
                        opt => opt.MapFrom(ps =>  ps.CustomerContact.EmailAddress))
                    .ForMember(psr => psr.TelephoneHome,                    
                        opt => opt.MapFrom(ps =>  ps.CustomerContact.TelephoneHome))
                    .ForMember(psr => psr.TelephoneWork,                    
                        opt => opt.MapFrom(ps =>  ps.CustomerContact.TelephoneWork))
                    .ForMember(psr => psr.TelephoneMobile,                    
                        opt => opt.MapFrom(ps =>  ps.CustomerContact.TelephoneMobile))
                    .ForMember(psr => psr.FullName, 
                        opt => opt.MapFrom(ps =>  ps.CustomerContact.FirstName  
                                            + ' ' + ps.CustomerContact.LastName))
                    //Address Details
                    .ForMember(psr => psr.AddressLine1,                    
                        opt => opt.MapFrom(ps =>  ps.CustomerAddress.AddressLine1))
                    .ForMember(psr => psr.AddressLine2,                    
                        opt => opt.MapFrom(ps =>  ps.CustomerAddress.AddressLine2))
                    .ForMember(psr => psr.Postcode,                    
                        opt => opt.MapFrom(ps =>  ps.CustomerAddress.Postcode))

                    //No of planning apps associated with customer
                     .ForMember(psr => psr.planningAppsCount,                    
                        opt => opt.MapFrom(ps =>  ps.planningApps.Count))                  

                    .ForMember(psr => psr.NameSummary,
                    opt => opt.MapFrom(ps =>  ps.CustomerContact.FirstName  
                                            + ' ' + ps.CustomerContact.LastName
                                            + ", " + ps.CustomerAddress.AddressLine1
                                            + ' ' + ps.CustomerAddress.AddressLine2
                                            + ", " + ps.CustomerAddress.Postcode ))

                    .ForMember(psr => psr.CustomerAddressSummary,
                    opt => opt.MapFrom(ps =>  ps.CustomerAddress.AddressLine1
                                            + ' ' + ps.CustomerAddress.AddressLine2
                                            + ", " + ps.CustomerAddress.Postcode ));
                                            
            CreateMap<CustomerResource, Customer>()

                //Contact Details
                .ForPath(d => d.CustomerContact.FirstName, 
                        o => o.MapFrom(s => s.FirstName))
                .ForPath(d => d.CustomerContact.LastName, 
                        o => o.MapFrom(s => s.LastName))
                .ForPath(d => d.CustomerContact.EmailAddress, 
                        o => o.MapFrom(s => s.EmailAddress))
                .ForPath(d => d.CustomerContact.TelephoneHome, 
                        o => o.MapFrom(s => s.TelephoneHome))
                .ForPath(d => d.CustomerContact.TelephoneWork, 
                        o => o.MapFrom(s => s.TelephoneWork))
                .ForPath(d => d.CustomerContact.TelephoneMobile, 
                        o => o.MapFrom(s => s.TelephoneMobile))

                //Address Details
                .ForPath(d => d.CustomerAddress.CompanyName, 
                        o => o.MapFrom(s => s.AddressLine1))
                .ForPath(d => d.CustomerAddress.AddressLine1, 
                        o => o.MapFrom(s => s.AddressLine1))
                .ForPath(d => d.CustomerAddress.AddressLine2, 
                        o => o.MapFrom(s => s.AddressLine2))
                .ForPath(d => d.CustomerAddress.Postcode, 
                        o => o.MapFrom(s => s.Postcode))
                .ForPath(d => d.CustomerAddress.GeoLocation, 
                        o => o.MapFrom(s => s.GeoLocation))
                .ForMember(psr => psr.SearchCriteria,
                    opt => opt.MapFrom(ps =>        ps.FirstName   
                                            + ' ' + ps.LastName 
                                            + ' ' + ps.AddressLine1
                                            + ' ' + ps.AddressLine2
                                            + ' ' + ps.Postcode
                                            + ' ' + ps.EmailAddress
                                            + ' ' + ps.TelephoneHome
                                            + ' ' + ps.TelephoneMobile));

            CreateMap<Customer, CustomerSelectResource>()
                .ForMember(psr => psr.Name,
                    opt => opt.MapFrom(ps => ps.CustomerContact.FirstName  
                                            + ' ' + ps.CustomerContact.LastName
                                            + ", " + ps.CustomerAddress.AddressLine1
                                            + ' ' + ps.CustomerAddress.AddressLine2
                                            + ", " + ps.CustomerAddress.Postcode ));
                                           
        }
    }
}