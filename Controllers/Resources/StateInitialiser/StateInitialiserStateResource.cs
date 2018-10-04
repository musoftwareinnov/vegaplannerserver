using System.Collections.Generic;
using vega.Core.Models;

namespace vega.Controllers.Resources.StateInitialser
{
    public class StateInitialiserStateResource
    {
        public int Id { get; set; }
        public string Name { get; set; }
        public int OrderId { get; set; }  
        public int CompletionTime { get; set; }          //Days
        public int AlertToCompletionTime { get; set; }   //Days
        public int StateInitialiserId { get; set; }
        public bool isDeleted { get; set; }
        public bool canDelete { get; set; }
        public ICollection<StateInitialiserCustomField> StateInitialiserStateCustomFields { get; set; }
    }
}