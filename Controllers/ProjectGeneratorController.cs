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

namespace vega.Controllers
{
    [Authorize(Policy = "ApiUser")]
    [Route("/api/projectgenerators")]
    public class ProjectGeneratorController : Controller
    {
        private readonly IMapper mapper;
        private readonly IProjectGeneratorRepository projectGeneratorRepository;
        private readonly IUnitOfWork unitOfWork;
        public ProjectGeneratorController(IMapper mapper, 
                                          IProjectGeneratorRepository projectGeneratorRepository, 
                                          IUnitOfWork unitOfWork)
        {
            this.unitOfWork = unitOfWork;
            this.mapper = mapper;
            this.projectGeneratorRepository = projectGeneratorRepository;
        }  

        [HttpGet]
        public async Task<IActionResult> ProjectGenerators(bool includeDeleted)   
        {
            var queryResult = await projectGeneratorRepository.GetProjectGenerators();
            var r = mapper.Map<QueryResult<ProjectGenerator>, QueryResultResource<ProjectGeneratorResource>>(queryResult);

            return Ok(r);
        }
    } 
}