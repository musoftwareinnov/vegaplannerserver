using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;
using vegaplannerserver.Core.Models;

namespace vega.Core.Models
{
    public class Contact
    {

        public int CustomerTitleId { get; set; }
        
        public string CustomerTitle { get; set; }
        
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