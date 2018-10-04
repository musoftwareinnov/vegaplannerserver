using System;
using vega.Controllers.Resources.Contact;
using vega.Core.Models;
using vegaplanner.Controllers.Resources;

namespace vega.Controllers.Resources
{
    public class UpdatePlanningAppResource
    {

        public int  method { get; set; }
        public int rollbackToStateId { get; set; }
        public string CurrentStateCompletionDate { get; set; }
        public string  DateCompleted { get; set; }
        public ContactResource Developer { get; set; }
        public AddressResource DevelopmentAddress { get; set; }
        public string Notes { get; set; }
    }
}