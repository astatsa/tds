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
    public class PositionsController : BaseTDSController
    {
        private readonly AppDbContext dbContext;

        public PositionsController(AppDbContext dbContext)
        {
            this.dbContext = dbContext;
        }

        [HttpGet]
        [Authorize(Roles = "ReferenceRead")]
        public async Task<ApiResult<List<DTO.Position>>> GetPositions()
        {
            try
            {
                return ApiResult(
                    await dbContext.Positions
                        .ProjectToType<DTO.Position>()
                        .ToListAsync());
            }
            catch(Exception ex)
            {
                return ApiResult<List<DTO.Position>>(null, $"{ex.Message}\n{ex.InnerException?.Message}");
            }
        }
    }
}