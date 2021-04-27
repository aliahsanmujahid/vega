using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using Microsoft.EntityFrameworkCore;
using API.Core;
using API.Core.Models;

namespace API.Persistence
{
  public class PhotoRepository : IPhotoRepository
  {
    private readonly VegaDbContext context;
    public PhotoRepository(VegaDbContext context)
    {
      this.context = context;
    }
    public async Task<IEnumerable<Photo>> GetPhotos(int NewVehicleId)
    {
      return await context.Photos
        .Where(p => p.NewVehicleId == NewVehicleId)
        .ToListAsync();
    }
  }
}