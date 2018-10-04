using System.ComponentModel.DataAnnotations.Schema;
using vega.Core.Models;
using vega.Core.Models.States;

namespace vega.Core.Models
{
    [Table("StateInitialiserStateCustomFields")]
    public class StateInitialiserStateCustomField
    {
        public int StateInitialiserStateId { get; set; }
        public StateInitialiserState StateInitialiserState { get; set; }
        public int StateInitialiserCustomFieldId { get; set; }

        public StateInitialiserCustomField StateInitialiserCustomField { get; set; }
    }
}