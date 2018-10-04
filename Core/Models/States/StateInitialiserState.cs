using System;
using System.Collections.Generic;
using System.Collections.ObjectModel;
using System.ComponentModel.DataAnnotations;
using vega.Core.Models.Generic;

namespace vega.Core.Models.States
{
    
    public class StateInitialiserState : IdNameProperty
    {
        public int OrderId { get; set; }  
        public int CompletionTime { get; set; }          //Days
        public int AlertToCompletionTime { get; set; }   //Days

        public int StateInitialiserId { get; set; }

        public bool isDeleted { get; set; }
        public StateInitialiser stateInitialiser;

        public bool canDelete { get; set; }
        public ICollection<StateInitialiserStateCustomField> StateInitialiserStateCustomFields { get; set; }

        public StateInitialiserState()
        {  
            StateInitialiserStateCustomFields = new Collection<StateInitialiserStateCustomField>();
            canDelete = true;
        }
    }
}