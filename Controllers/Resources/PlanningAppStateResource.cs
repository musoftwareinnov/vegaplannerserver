using System;
using System.Collections.Generic;
using AutoMapper;
using vega.Core.Models;

namespace vega.Controllers.Resources
{
    public class PlanningAppStateResource
    {
        public int Id { get; set; }
        public int GeneratorId { get; set; }
        public int GeneratorOrder { get; set; } 
        public string StateName { get; set; }
        public string DueByDate { get; set; }
        public string DateCompleted { get; set; }
        public string StateStatus { get; set; }
        public bool CurrentState { get; set; }
        public bool isCustomDuraton  { get; set; }
        public bool isLastGeneratorState  { get; set; }
        public bool mandatoryFieldsSet { get; set; }
    }
}