using System;
using System.Collections.Generic;
using System.Linq;
using System.Linq.Expressions;
using System.Threading.Tasks;
using API.Core;
using API.Core.Models;
using API.Extensions;
using Microsoft.EntityFrameworkCore;

namespace API.Persistence
{
    public class VehicleRepository : IVehicleRepository
    {
        private readonly VegaDbContext _context;

        public VehicleRepository(VegaDbContext context)
        {
            _context = context;

        }
        public async Task<NewVehicle> GetVehicle(int id, bool includeRelated = true)
        {
            if (!includeRelated)
              return await _context.Vehicles.FindAsync(id);

            return await _context.Vehicles
              .Include(v => v.Features)
                .ThenInclude(vf => vf.Feature)
              .Include(v => v.Model)
                .ThenInclude(m => m.Make)
              .SingleOrDefaultAsync(v => v.Id == id);
        }

        public async Task<QueryResult<NewVehicle>> GetVehicles(VehicleQuery queryObj)
        {
           var result = new QueryResult<NewVehicle>();
           var query = _context.Vehicles
              .Include(v => v.Model)
                .ThenInclude(m => m.Make)
              .Include(v => v.Features)
                .ThenInclude(vf => vf.Feature)
                .AsQueryable();   

           if(queryObj.MakeId.HasValue){
              query = query.Where(v => v.Model.MakeId == queryObj.MakeId.Value);
            } 
            if(queryObj.ModelId.HasValue){
              query = query.Where(v => v.ModelId == queryObj.ModelId.Value);
            }   

            var columnsMap = new Dictionary<string, Expression<Func<NewVehicle, object>>>()
            {
              ["make"] = v => v.Model.Make.Name,
              ["model"] = v => v.Model.Name,
              ["contactName"] = v => v.ContactName
            };

            query = query.ApplyOrdering(queryObj, columnsMap);
            result.TotalItems = await query.CountAsync();
            query = query.ApplyPaging(queryObj);
            
                        
            result.Items = await query.ToListAsync();

            return result;    

        }

        public void Remove(NewVehicle vehicle)
        {
            _context.Remove(vehicle);
        }
        public void Add(NewVehicle vehicle)
        {
            _context.Vehicles.Add(vehicle);
        }
    }
}