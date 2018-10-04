namespace vega.Controllers.Resources
{
    public class PlanningCustomerResource
    {
        public int Id { get; set; }
        public string FirstName { get; set;}
        public string LastName { get; set; }
        public string AddressLine1 { get; set; }
        public string AddressLine2 { get; set; }
        public string Postcode { get; set; }
        public string EmailAddress { get; set; }
        public string TelephoneHome { get; set; }
        public string TelephoneMobile { get; set; }

        public string TelephoneWork { get; set; }
        public string Notes { get; set; }
    }
}