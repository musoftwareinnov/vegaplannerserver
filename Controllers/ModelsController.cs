using System.Collections.Generic;
using System.Threading.Tasks;
using AutoMapper;
using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;
using vega.Controllers.Resources;
using vega.Core.Models;
using vega.Persistence;

namespace vega.Controllers
{
    public class ModelsController  : Controller
    {
        private readonly VegaDbContext context;
        public ModelsController(VegaDbContext context, IMapper mapper)
        {
            this.context = context;
        }

        [HttpGet("/api/models")]
        public async Task<IEnumerable<ModelResource>> GetModels()
        {
            var models = await context.Models.ToListAsync();
            return Mapper.Map<List<Model>, List<ModelResource>>(models);
        }
    }
}