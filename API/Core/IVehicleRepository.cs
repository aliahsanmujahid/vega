using System.Collections.Generic;
using System.Threading.Tasks;
using API.Core.Models;

namespace API.Core
{
    public interface IVehicleRepository
    {
        Task<NewVehicle> GetVehicle(int id, bool includeRelated = true);
        Task<QueryResult<NewVehicle>> GetVehicles(VehicleQuery filter);
        void Add(NewVehicle vehicle);
        void Remove(NewVehicle vehicle);
    }
}