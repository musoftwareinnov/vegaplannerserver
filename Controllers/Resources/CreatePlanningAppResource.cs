using System.Collections.Generic;
using System.Collections.ObjectModel;
using vega.Controllers.Resources.Contact;
using vega.Core.Models;

namespace vega.Controllers.Resources
{
    public class CreatePlanningAppResource
    {
        public int StateInitialiserId { get; set; }
        public int ProjectGeneratorId { get; set; }
        public int CustomerId { get; set; }
        public string DescriptionOfWork { get; set; }
        public string Name { get; set; }
        public ContactResource Developer { get; set; }
        public Address DevelopmentAddress { get; set; }
        public string Notes { get; set; }
        public ICollection<string> Surveyors { get; set; }
        public ICollection<string> Drawers { get; set; }
        public ICollection<string> Admins { get; set; }

        public CreatePlanningAppResource()
        {
            Developer = new ContactResource();
            DevelopmentAddress = new Address();
            Surveyors = new Collection<string>();
            Drawers = new Collection<string>();
            Admins = new Collection<string>();
        }
    }


}