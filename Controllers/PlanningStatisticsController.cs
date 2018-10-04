using System;
using System.Collections.Generic;
using System.Threading.Tasks;
using AutoMapper;
using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;
using vega.Controllers.Resources;
using vega.Core.Models;
using vega.Core;
using System.Globalization;
using vega.Extensions.DateTime;
using Microsoft.Extensions.Options;
using vega.Core.Models.Settings;
using vega.Core.Utils;
using vega.Controllers.Resources.Statistics;
using Microsoft.AspNetCore.Authorization;

namespace vegaplanner.Controllers
{
    [Route("/api/planningappstatistics")]
    public class PlanningStatisticsController 
    {        private readonly IMapper mapper;
        private readonly IPlanningStatisticsRepository repository;
        private readonly IUnitOfWork unitOfWork;
        public PlanningStatisticsController(IMapper mapper, 
                                     IPlanningStatisticsRepository repository, 
                                     IUnitOfWork unitOfWork )
        {
            this.mapper = mapper;
            this.repository = repository;
            this.unitOfWork = unitOfWork;
        }

        [HttpGet]
        public PlanningStatisticsResource GetPlanningStatistics()     
        {   
            var results = repository.getPlanningStatistics();
    
        
            return mapper.Map<PlanningStatistics, PlanningStatisticsResource>(results);             
        }
    }
}