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
using Microsoft.AspNetCore.Authorization;
using vegaplanner.Core.Models.Security.Helpers;
using vegaplannerserver.Controllers.Resources;
using vegaplannerserver.Core.Models;
using vegaplannerserver.Core;

namespace vega.Controllers
{
    [Authorize(Policy = "ApiUser")]
    [Route("/api/descriptionsofwork")]
    public class DescriptionOfWorkController : Controller
    {
        private readonly IMapper mapper;
        private readonly IDescriptionOfWorkRepository descriptionOfWorkRepository;
        private readonly IUnitOfWork unitOfWork;
        public DescriptionOfWorkController(IMapper mapper, IDescriptionOfWorkRepository descriptionOfWorkRepository, IUnitOfWork unitOfWork)
        {
            this.unitOfWork = unitOfWork;
            this.descriptionOfWorkRepository = descriptionOfWorkRepository;
            this.mapper = mapper;
        }

        [HttpGet]
        public async Task<QueryResultResource<DescriptionOfWorkResource>> GetDescriptionsOfWork()     
        {
            
            var queryResult = await descriptionOfWorkRepository.GetDescriptionsOfWork();
    
            return mapper.Map<QueryResult<DescriptionOfWork>, QueryResultResource<DescriptionOfWorkResource>>(queryResult);             
        }
    }
}