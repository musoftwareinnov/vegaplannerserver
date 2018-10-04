using System;
using System.Collections;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Threading.Tasks;
using AutoMapper;
using Microsoft.AspNetCore.Hosting;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;
using Microsoft.Extensions.Options;
using vega.Controllers.Resources;
using vega.Core;
using vega.Core.Models;

namespace vega.Controllers
{
    [Route("/api/vehicles/{vehicleId}/photos")]
    public class PhotosController : Controller
    {
        private readonly IHostingEnvironment host;
        private readonly IMapper mapper;
        private readonly IUnitOfWork unitOfWork;
        private readonly IVehicleRepository repository;
        private readonly PhotoSettings photoSettings;

        private IPhotoRepository photoRepository;
        public PhotosController(IHostingEnvironment host, IVehicleRepository repository,
                                IPhotoRepository photoRepository,
                                IMapper mapper, 
                                IUnitOfWork unitOfWork,
                                IOptionsSnapshot<PhotoSettings> options)
        {
            this.host = host;
            this.repository = repository;
            this.photoRepository = photoRepository;
            this.unitOfWork = unitOfWork;
            this.mapper = mapper;
            this.photoSettings = options.Value;
        }

        [HttpGet]
        public async Task<IEnumerable<PhotoResource>> GetPhotos(int vehicleId) {
            var photos = await photoRepository.GetPhotos(vehicleId);

            return mapper.Map<IEnumerable<Photo>, IEnumerable<PhotoResource>>(photos);
        }

        [HttpPost]
        [RequestSizeLimit(100_000_000)]   //Kestrel default limit is 30MB!
        public async Task<IActionResult> Upload(int vehicleId, IFormFile file)
        {   
            var vehicle = await repository.GetVehicle(vehicleId, includeRelated:false);
            if(vehicle == null)
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
            var photo = new Photo { FileName = fileName };
            vehicle.Photos.Add(photo);
            await unitOfWork.CompleteAsync();

            return Ok(mapper.Map<Photo, PhotoResource>(photo));
        }
        
    }
}