using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;
using TDSServer.Models;

namespace TDSServer.Controllers
{
    [Route("api/[controller]")]
    [ApiController]
    [Authorize]
    public class GasStationsController : ControllerBase
    {
        private readonly AppDbContext dbContext;
        public GasStationsController(AppDbContext dbContext)
        {
            this.dbContext = dbContext;
        }

        [HttpGet]
        public async Task<ApiResult<IEnumerable<GasStation>>> GetGasStations()
        {
            try
            {
                return new ApiResult<IEnumerable<GasStation>>(await dbContext
                    .GasStations
                    .Where(x => x.IsDeleted != true)
                    .ToListAsync());
            }
            catch(Exception ex)
            {
                return new ApiResult<IEnumerable<GasStation>>(null, ex.Message);
            }
        }
    }
}