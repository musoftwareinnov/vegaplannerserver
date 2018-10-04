using System;
using System.ComponentModel.DataAnnotations;

namespace vega.Core.Models.Generic
{ 
    public class IdNameProperty
    {
        public int Id { get; set; }

        [Required]
        [StringLength(255)]
        public string Name { get; set;}

        public DateTime LastUpdate { get; set; }
    }
}