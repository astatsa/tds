using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using Mapster;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;
using TDSDTO;
using TDSServer.Models;
using DTO = TDSDTO.References;

namespace TDSServer.Controllers
{
    [Route("api/[controller]")]
    [ApiController]
    [Authorize]
    public class GasStationsController : BaseTDSController
    {
        private readonly AppDbContext dbContext;
        public GasStationsController(AppDbContext dbContext)
        {
            this.dbContext = dbContext;
        }

        [HttpGet("actual")]
        [Authorize(Roles = "MobileApp")]
        public async Task<ApiResult<List<DTO.GasStation>>> GetActualGasStations()
        {
            try
            {
                return ApiResult(await dbContext
                    .GasStations
                    .Where(x => x.IsDeleted != true)
                    .ProjectToType<DTO.GasStation>()
                    .ToListAsync());
            }
            catch(Exception ex)
            {
                return new ApiResult<List<DTO.GasStation>>(null, $"{ex.Message}\n{ex.InnerException?.Message}");
            }
        }

        [HttpGet]
        [Authorize(Roles = "ReferenceRead")]
        public async Task<ApiResult<List<DTO.GasStation>>> GetGasStations()
        {
            try
            {
                return ApiResult(
                    await dbContext.GasStations
                    .ProjectToType<DTO.GasStation>()
                    .ToListAsync());
            }
            catch(Exception ex)
            {
                return ApiResult<List<DTO.GasStation>>(null, $"{ex.Message}\n{ex.InnerException?.Message}");
            }
        }
    }
}