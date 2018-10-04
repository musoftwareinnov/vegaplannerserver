using System;
using System.Collections.Generic;
using System.Threading.Tasks;
using AutoMapper;
using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;
using vega.Controllers.Resources;
using vega.Core.Models;
using vega.Core;
using vega.Core.Models.States;
using Microsoft.AspNetCore.Authorization;

namespace vega.Controllers
{   
    [Authorize(Policy = "ApiUser")]
    [Route("/api/statestatus")]
    public class StateStatusController: Controller
    {  
        private readonly IMapper mapper;
        private readonly IStateStatusRepository repository;
        private readonly IUnitOfWork unitOfWork;
        public StateStatusController(IMapper mapper, IStateStatusRepository repository, IUnitOfWork unitOfWork)
        {
            this.unitOfWork = unitOfWork;
            this.repository = repository;
            this.mapper = mapper;
        }

        [HttpGet("{customerId}")]
        public IEnumerable<StateStatusResource> GetCustomerStatuses(int customerId)
        {
            var statuses = repository.GetStateStatusListCustomer(customerId);

            return Mapper.Map<List<StateStatus>, List<StateStatusResource>>(statuses);
        }

        [HttpGet]
        public async Task<IEnumerable<StateStatusResource>> GetStatuses(string StatusName)
        {
            var statuses = await repository.GetStateStatusList(StatusName);

            return Mapper.Map<List<StateStatus>, List<StateStatusResource>>(statuses);
        }


    }
}