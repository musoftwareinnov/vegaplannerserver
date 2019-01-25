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
using vegaplannerserver.Controllers.Resources.Contact;
using vegaplannerserver.Core;
using vegaplannerserver.Core.Models;

namespace vega.Controllers
{
    [Authorize(Policy = "ApiUser")]
    [Route("/api/staticdata")]
    public class StaticDataController : Controller
    {
        private readonly IMapper mapper;
        private readonly IStaticDataRepository staticDataRepository;
        public StaticDataController(IMapper mapper, IStaticDataRepository staticDataRepository)
        {
            this.staticDataRepository = staticDataRepository;
            this.mapper = mapper;
        }

        [HttpGet("honorifics")]
        public async Task<List<KeyValuePairResource>> GetHonorifics()     
        {   
            var queryResult = await staticDataRepository.GetTitles();
    
            return mapper.Map<List<Title>, List<KeyValuePairResource>>(queryResult);             
        }
    }
}