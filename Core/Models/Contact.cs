using System.ComponentModel.DataAnnotations;
using vegaplannerserver.Core.Models;

namespace vega.Core.Models
{
    public class Contact
    {
        public Title Title { get; set; }
        
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