using System;
using System.Collections.Generic;
using System.Threading.Tasks;
using API.Persistence;
using AutoMapper;
using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;
using API.Controllers.Resources;
using API.Core;
using API.Core.Models;

namespace API.Controllers
{
    [Route("api/[controller]")]
    public class VehiclesController : Controller
    {
        private readonly IVehicleRepository _repository;
        private readonly IMapper _mapper;
        private readonly IUnitOfWork _unitOfWork;

        public VehiclesController(IVehicleRepository repository, IMapper mapper, IUnitOfWork unitOfWork)
        {
            _repository = repository;
            _mapper = mapper;
            _unitOfWork = unitOfWork;
        }

        [HttpPost]
        public async Task<IActionResult> CreateVehicle([FromBody] SaveVehicleResource vehicleResource)
        {

            var vehicle = _mapper.Map<SaveVehicleResource, NewVehicle>(vehicleResource);
            vehicle.LastUpdate = DateTime.Now;

            _repository.Add(vehicle);
            await _unitOfWork.CompleteAsync();
            
            vehicle = await _repository.GetVehicle(vehicle.Id);

            var result = _mapper.Map<NewVehicle, VehicleResource>(vehicle);
           
            return Ok(result);
        }

        [HttpPut("{id}")]
        public async Task<IActionResult> UpdateVehicle(int id, [FromBody] SaveVehicleResource vehicleResource)
        {
            if (!ModelState.IsValid)
                return BadRequest(ModelState);

            var vehicle = await _repository.GetVehicle(id);

            if (vehicle == null)
                return NotFound();

            _mapper.Map<SaveVehicleResource, NewVehicle>(vehicleResource, vehicle);
            vehicle.LastUpdate = DateTime.Now;

            await _unitOfWork.CompleteAsync();
            
            vehicle = await _repository.GetVehicle(vehicle.Id);

            var result = _mapper.Map<NewVehicle, VehicleResource>(vehicle);
           
            return Ok(result);
        }

        [HttpDelete("{id}")]
        public async Task<IActionResult> DeleteVehicle(int id)
        {
            var vehicle = await _repository.GetVehicle(id, includeRelated: false);

            if (vehicle == null)
                return NotFound();

            _repository.Remove(vehicle);
            await _unitOfWork.CompleteAsync();
            return Ok(id);
        }

        [HttpGet("{id}")]
        public async Task<IActionResult> GetVehicle(int id)
        {
            var vehicle = await _repository.GetVehicle(id);

            if (vehicle == null)
                return NotFound();

            var vehicleResource = _mapper.Map<NewVehicle, VehicleResource>(vehicle);

            return Ok(vehicleResource);
        }

    [HttpGet]
    public async Task<QueryResultResource<VehicleResource>> GetVehicles(VehicleQueryResource filterResource)
    {
      var filter = _mapper.Map<VehicleQueryResource, VehicleQuery>(filterResource);
      var queryResult = await _repository.GetVehicles(filter);

      return _mapper.Map<QueryResult<NewVehicle>, QueryResultResource<VehicleResource>>(queryResult);
    }


    }
}