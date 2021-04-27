using System;
using System.IO;
using System.Threading.Tasks;
using API.Core;
using AutoMapper;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;
using Microsoft.Extensions.Hosting;
using Microsoft.Extensions.Options;
using API.Controllers.Resources;
using API.Core.Models;
using API.Persistence;
using System.Collections.Generic;

namespace API.Controllers
{
    [Route("/api/vehicles/photos/{wewe}")]
    public class PhotosController : Controller
    {
        private readonly IPhotoRepository photoRepository;
        private readonly VegaDbContext context;
        private readonly IMapper mapper;
        private readonly IHostEnvironment host;
        private readonly IVehicleRepository vehicleRepository;
        private readonly IUnitOfWork unitOfWork;
        private readonly PhotoSettings photoSettings;
        public PhotosController(IPhotoRepository photoRepository, VegaDbContext context,IOptionsSnapshot<PhotoSettings> options, IMapper mapper, IHostEnvironment host, IVehicleRepository vehicleRepository, IUnitOfWork unitOfWork)
        {
            this.photoSettings = options.Value;
            this.photoRepository = photoRepository;
            this.context = context;
            this.mapper = mapper;
            this.host = host;
            this.vehicleRepository = vehicleRepository;
            
        }

        [HttpGet]
        public async Task<IEnumerable<PhotoResource>> GetPhotos(int wewe)
        {
            var photos = await photoRepository.GetPhotos(wewe);

            return mapper.Map<IEnumerable<Photo>, IEnumerable<PhotoResource>>(photos);
        }

        

        [HttpPost]
        public async Task<IActionResult> Upload(IFormFile file, int wewe)
        {

            var vehicle = await vehicleRepository.GetVehicle(wewe, includeRelated: false);

            if(vehicle == null){
                return NotFound();
            }
            if(file == null){
                return BadRequest("Null File");
            }
            if(file.Length == 0){
                return BadRequest("Null File");
            }
            if(file.Length > photoSettings.MaxBytes){
                return BadRequest("Large File");
            }
            if(!photoSettings.IsSupported(file.FileName)){
                return BadRequest("File Type Not Supported");
            }
            var uploadsFolderPath = Path.Combine(Directory.GetCurrentDirectory(), "uploads");
        
            if(!Directory.Exists(uploadsFolderPath))
                 Directory.CreateDirectory(uploadsFolderPath);
            
            var filename = Guid.NewGuid().ToString() + Path.GetExtension(file.FileName);
  
            var filePath = Path.Combine(uploadsFolderPath,filename);


            using (var stream = new FileStream(filePath, FileMode.Create))
            {
                await file.CopyToAsync(stream);
            }
            
            var photo = new Photo{FileName = filename};

            vehicle.Photos.Add(photo);

            await context.SaveChangesAsync();

            return Ok(mapper.Map<Photo, PhotoResource>(photo));


        }
    }
}