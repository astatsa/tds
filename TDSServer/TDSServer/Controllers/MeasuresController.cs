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
    public class MeasuresController : BaseReferenceController<Models.Measure, DTO.Measure>
    {
        public MeasuresController(AppDbContext dbContext) : base(dbContext)
        {
        }

        [Authorize(Roles = "ReferenceRead")]
        [HttpGet]
        public async Task<ApiResult<List<DTO.Measure>>> GetMeasures()
        {
            try
            {
                return ApiResult(
                    await dbContext
                    .Measures
                    .ProjectToType<DTO.Measure>()
                    .ToListAsync());
            }
            catch(Exception ex)
            {
                return ApiResult<List<DTO.Measure>>(null, $"{ex.Message}\n{ex.InnerException?.Message}");
            }
        }

        [HttpPost]
        [Authorize(Roles = "ReferenceEdit")]
        public Task<ApiResult<bool>> SaveMeasures([FromBody] DTO.Measure measure)
            => Save(measure);
    }
}