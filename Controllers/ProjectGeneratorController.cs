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

        public IStateInitialiserRepository StateInitialiserRepository { get; }

        public ProjectGeneratorController(IMapper mapper, 
                                          IProjectGeneratorRepository projectGeneratorRepository, 
                                          IStateInitialiserRepository stateInitialiserRepository,
                                          IUnitOfWork unitOfWork)
        {
            this.unitOfWork = unitOfWork;
            this.mapper = mapper;
            this.projectGeneratorRepository = projectGeneratorRepository;
            this.StateInitialiserRepository = stateInitialiserRepository;
        }  

        [HttpGet]
        public async Task<IActionResult> ProjectGenerators()   
        {
            var queryResult = await projectGeneratorRepository.GetProjectGenerators();
            var result = mapper.Map<QueryResult<ProjectGenerator>, QueryResultResource<ProjectGeneratorResource>>(queryResult);

            return Ok(result);
        }

       [HttpGet("{id}")]
        public async Task<IActionResult> GetProjectGenerator(int id)   
        {
            var queryResult = await projectGeneratorRepository.GetProjectGenerator(id);
            var result = mapper.Map<ProjectGenerator, ProjectGeneratorSummaryResource>(queryResult);

            return Ok(result);
        }

        [HttpPost]       
        public async Task<IActionResult> CreateProjectGenerator([FromBody] ProjectGeneratorCreateResource projectGeneratorResource)   
        {

            var projectGenerator = new ProjectGenerator { Name = projectGeneratorResource.Name };
            projectGeneratorRepository.Add(projectGenerator);
            await unitOfWork.CompleteAsync();
            
            var result = mapper.Map<ProjectGenerator, ProjectGeneratorResource>(projectGenerator);


            return Ok(result);
        }
        [HttpPut]       
        public async Task<IActionResult> AppendProjectGenerator([FromBody] ProjectGeneratorAppendResource projectGeneratorResource)   
        {
            var projectGenerator = await projectGeneratorRepository.GetProjectGenerator(projectGeneratorResource.Id);
            var generatorToAdd = await StateInitialiserRepository.GetStateInitialiser(projectGeneratorResource.GeneratorId);

            projectGeneratorRepository.InsertGenerator( projectGenerator, 
                                                        generatorToAdd,
                                                        projectGeneratorResource.OrderId );
            await unitOfWork.CompleteAsync();

            var result = mapper.Map<ProjectGenerator, ProjectGeneratorResource>(projectGenerator);

            return Ok(result);
        }
    } 
}