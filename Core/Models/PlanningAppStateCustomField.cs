using System;
using System.ComponentModel.DataAnnotations.Schema;

namespace vega.Core.Models
{
    [Table("PlanningAppStateCustomFields")]
    public class PlanningAppStateCustomField
    {
        public int Id { get; set; }
        public int StateInitialiserStateCustomFieldId { get; set; }
        public string StrValue { get; set;}
        public int IntValue { get; set;}
        public DateTime DateValue { get; set;}
    }
}