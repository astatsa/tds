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
    public class MeasuresController : BaseTDSController
    {
        private readonly AppDbContext dbContext;

        public MeasuresController(AppDbContext dbContext)
        {
            this.dbContext = dbContext;
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
        public async Task<ApiResult<bool>> SaveMeasures([FromBody] DTO.Measure measure)
        {
            try
            {
                Measure m;
                if (measure.Id != default)
                {
                    m = await dbContext.Measures.
                        FirstOrDefaultAsync(x => x.Id == measure.Id);
                    if (m == null)
                    {
                        return new ApiResult<bool>(false, "Элемент справочника не найден!");
                    }
                }
                else
                {
                    m = new Measure();
                    dbContext.Add(m);
                }

                m.Name = measure.Name;
                m.FullName = measure.FullName;

                await dbContext.SaveChangesAsync();
            }
            catch (Exception ex)
            {
                return new ApiResult<bool>(false, ex.Message);
            }
            return new ApiResult<bool>(true);
        }
    }
}