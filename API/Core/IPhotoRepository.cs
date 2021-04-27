using System.Collections.Generic;
using System.Threading.Tasks;
using API.Core.Models;

namespace API.Core
{
    public interface IPhotoRepository
    {
         Task<IEnumerable<Photo>> GetPhotos(int vehicleId);
    }
}