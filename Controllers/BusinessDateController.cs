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
using vega.Extensions.DateTime;
using vega.Core.Utils;
using vega.Services.Interfaces;

namespace vega.Controllers
{
    [Authorize(Policy = "ApiUser")]
    [Route("/api/businessdates")]
    public class BusinessDateController : Controller
    {
        private readonly IMapper mapper;
        private readonly IBusinessDateRepository businessDateRepository;
        private readonly IUnitOfWork unitOfWork;
        public BusinessDateController(IMapper mapper, 
                                        IBusinessDateRepository businessDateRepository,
                                        IDateService dateService,
                                        IUnitOfWork unitOfWork)
        {
            this.unitOfWork = unitOfWork;
            this.businessDateRepository = businessDateRepository;
            this.DateService = dateService;
            this.mapper = mapper;
        }

        public IDateService DateService { get; }

        [HttpGet]
        public async Task<BusinessDateResource> GetBusinessDates()
        {
            var businessDates = await businessDateRepository.GetBusinessDate();

            return mapper.Map<BusinessDate, BusinessDateResource>(businessDates);
        }

        [HttpPut("{date}")]
        public async Task<BusinessDateResource> SetBusinessDate(string date)
        {
            DateTime businessDate = date.ParseInputDate();
            DateService.SetCurrentDate(businessDate);
            return await GetBusinessDates();
        }
    }
}