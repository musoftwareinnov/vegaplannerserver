using System;
using System.Collections.Generic;
using System.Collections.ObjectModel;
using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;
using System.Linq;
using vega.Core.Models.Generic;
using vega.Core.Models.States;

namespace vega.Core.Models
{  
    public class StateInitialiser : IdNameProperty
    {  
        public int OrderId { get; set; }
        public string Description { get; set; }
        public List<StateInitialiserState> States { get; set; }

        [NotMapped]
        public List<StateInitialiserState> OrderedStates {
            get { return this.States.OrderBy(o => o.OrderId).ToList(); } 
        }
        public StateInitialiser()
        {  
            States = new List<StateInitialiserState>();

        }
    }
}