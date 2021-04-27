using System.Collections.Generic;
using System.Threading.Tasks;
using API.Controllers.Resources;
using API.Core.Models;
using API.Persistence;
using AutoMapper;
using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;

namespace API.Controllers
{
  public class FeaturesController : Controller
  {
    private readonly VegaDbContext context;
    private readonly IMapper mapper;
    public FeaturesController(VegaDbContext context, IMapper mapper)
    {
      this.mapper = mapper;
      this.context = context;
    }

    [HttpGet("/api/features")]
    public async Task<IEnumerable<KeyValuePairResource>> GetFeatures()
    {
      var features = await context.Features.ToListAsync();
      
      return mapper.Map<List<Feature>, List<KeyValuePairResource>>(features); 
    }
  }
}