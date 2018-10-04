using vega.Controllers.Resources.Contact;
using vega.Core.Models;

namespace vega.Controllers.Resources
{
    public class CreatePlanningAppResource
    {
        public int StateInitialiserId { get; set; }
        public int CustomerId { get; set; }
        public string Name { get; set; }
        public ContactResource Developer { get; set; }
        public Address DevelopmentAddress { get; set; }


        public CreatePlanningAppResource()
        {
            Developer = new ContactResource();
            DevelopmentAddress = new Address();
        }
    }


}