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
    public class FeatureController: Controller
    {  
        private readonly VegaDbContext context;
        public FeatureController(VegaDbContext context, IMapper mapper)
        {
            this.context = context;
        }
 
        [HttpGet("/api/features")]
        public async Task<IEnumerable<KeyValuePairResource>> GetMakes()
        {
            var features = await context.Features.ToListAsync();
            return Mapper.Map<List<Feature>, List<KeyValuePairResource>>(features);
        }
    }
}