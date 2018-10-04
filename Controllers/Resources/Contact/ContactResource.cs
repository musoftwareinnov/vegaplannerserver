using System.ComponentModel.DataAnnotations;

namespace vega.Controllers.Resources.Contact
{
    public class ContactResource
    {
        [StringLength(30)]
        public string FirstName { get; set;}
        [StringLength(30)]
        public string LastName { get; set;}
        [StringLength(30)]
        public string FullName { get; set;}
        [StringLength(30)]
        public string TelephoneWork { get; set; }
        [StringLength(30)]
        public string TelephoneMobile { get; set; }
        [StringLength(30)]
        public string TelephoneHome { get; set; }
        [StringLength(30)]
        public string EmailAddress { get; set; }
    }
}