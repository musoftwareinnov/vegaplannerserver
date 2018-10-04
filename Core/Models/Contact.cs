using System.ComponentModel.DataAnnotations;

namespace vega.Core.Models
{
    public class Contact
    {

        [StringLength(30)]
        public string FirstName { get; set;}
        [StringLength(30)]
        public string LastName { get; set;}

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