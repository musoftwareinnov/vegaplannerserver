using System;
using System.Collections;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Threading.Tasks;
using AutoMapper;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Hosting;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;
using Microsoft.Extensions.Options;
using vega.Controllers.Resources;
using vega.Core;
using vega.Core.Models;
 
namespace vega.Controllers
{
    // [Authorize(Policy = "ApiUser")]
    [Route("/api/planningApps/{planningAppId}/drawings")]
    public class DrawingsController : Controller
    {
        private readonly IHostingEnvironment host;
        private readonly IMapper mapper;
        private readonly IUnitOfWork unitOfWork;
        private readonly IPlanningAppRepository repository;
        private readonly PhotoSettings photoSettings;

        private IDrawingRepository drawingRepository;
        public DrawingsController(IHostingEnvironment host, IPlanningAppRepository repository,
                                IDrawingRepository drawingRepository,
                                IMapper mapper, 
                                IUnitOfWork unitOfWork,
                                IOptionsSnapshot<PhotoSettings> options)
        {
            this.host = host;
            this.repository = repository;
            this.drawingRepository = drawingRepository;
            this.unitOfWork = unitOfWork;
            this.mapper = mapper;
            this.photoSettings = options.Value;
        }

        [HttpGet]
        public async Task<IEnumerable<DrawingResource>> GetDrawings(int planningAppId) {
            var drawings = await drawingRepository.GetDrawings(planningAppId);

            return mapper.Map<IEnumerable<Drawing>, IEnumerable<DrawingResource>>(drawings);
        }

        [HttpPost]
        [RequestSizeLimit(100_000_000)]   //Kestrel default limit is 30MB!
        public async Task<IActionResult> Upload(int planningAppId, IFormFile file)
        {   
            var planningApp = await repository.GetPlanningApp(planningAppId, includeRelated:false);
            if(planningApp == null)
                return NotFound();

            if(file == null) return BadRequest("Null file");
            if(file.Length == 0) return BadRequest("Empty File");
            if(file.Length > photoSettings.MaxBytes) return BadRequest("File too large");
            if(!photoSettings.IsSupported(file.FileName))
                return BadRequest("Bad filetype");

            var uploadsFolderPath = Path.Combine(host.WebRootPath, "uploads");
            if(!Directory.Exists(uploadsFolderPath))
                Directory.CreateDirectory(uploadsFolderPath);

            var fileName = Guid.NewGuid().ToString() + Path.GetExtension(file.FileName);
            var filePath = Path.Combine(uploadsFolderPath, fileName);

            using(var stream = new FileStream(filePath, FileMode.Create))
            {
                await file.CopyToAsync(stream);
            }
            //Generate thumbnails - System.Drawing.Namespace!
            var drawing = new Drawing { FileName = fileName };
            planningApp.Drawings.Add(drawing);
            await unitOfWork.CompleteAsync();

            return Ok(mapper.Map<Drawing, DrawingResource>(drawing));
        }
        
    }
}