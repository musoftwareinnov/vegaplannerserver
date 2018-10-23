using System.ComponentModel.DataAnnotations;

namespace vegaplanner.Controllers.Resources
{
    public class AddressResource
    {
        public string CompanyName { get; set; }
        [StringLength(255)]
        public string AddressLine1 { get; set; }
        public string City { get; set; }
        public string County { get; set; }

        [StringLength(10)]
        public string Postcode { get; set; }

        [StringLength(20)]
        public string GeoLocation { get; set; }
    }
}