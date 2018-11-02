using System.Collections.Generic;
using System.Threading.Tasks;
using AutoMapper;
using Microsoft.AspNetCore.Mvc;
using vega.Controllers.Resources;
using vega.Core;
using vega.Core.Models;
using vega.Core.Models.States;
using vega.Controllers.Resources.StateInitialser;
using System;
using Microsoft.AspNetCore.Authorization;
using vegaplannerserver.Core;
using vegaplannerserver.Core.Models.Settings;
using vegaplannerserver.Controllers.Resources;

namespace vega.Controllers
{
    [Authorize(Policy = "ApiUser")]
    [Route("/api/businessdates")]
    public class BusinessDateController : Controller
    {
        private readonly IMapper mapper;
        private readonly IBusinessDateRepository businessDateRepository;
        private readonly IUnitOfWork unitOfWork;
        public BusinessDateController(IMapper mapper, IBusinessDateRepository businessDateRepository, IUnitOfWork unitOfWork)
        {
            this.unitOfWork = unitOfWork;
            this.businessDateRepository = businessDateRepository;
            this.mapper = mapper;
        }  

        [HttpGet]
        public async Task<BusinessDateResource> GetBusinessDates()
        {
            var businessDates = await businessDateRepository.GetBusinessDate();

            return mapper.Map<BusinessDate, BusinessDateResource>(businessDates);
        }
    }
}