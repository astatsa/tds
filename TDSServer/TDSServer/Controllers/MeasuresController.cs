using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;
using TDSServer.Models;
using DTO = TDSDTO.References;

namespace TDSServer.Controllers
{
    [Route("api/[controller]")]
    [ApiController]
    [Authorize]
    public class MeasuresController : ControllerBase
    {
        private readonly AppDbContext dbContext;

        public MeasuresController(AppDbContext dbContext)
        {
            this.dbContext = dbContext;
        }

        [Authorize(Roles = "ReferenceRead")]
        [HttpGet]
        public Task<List<DTO.Measure>> GetMeasures() =>
            dbContext
            .Measures
            .Select(x => new DTO.Measure
            {
                Id = x.Id,
                FullName = x.FullName,
                IsDeleted = x.IsDeleted,
                Name = x.Name
            })
            .ToListAsync();

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