using System.Threading.Tasks;
using API.Core;

namespace API.Persistence
{
  public class UnitOfWork : IUnitOfWork
  {
    private readonly VegaDbContext _context;

    public UnitOfWork(VegaDbContext context)
    {
      _context = context;
    }

    public async Task CompleteAsync()
    {
      await _context.SaveChangesAsync();
    }
  }
}