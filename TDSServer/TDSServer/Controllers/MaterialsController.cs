using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
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
    public class MaterialsController : BaseTDSController
    {
        private readonly AppDbContext dbContext;

        public MaterialsController(AppDbContext dbContext)
        {
            this.dbContext = dbContext;
        }

        [HttpGet]
        [Authorize(Roles = "ReferenceRead")]
        public async Task<ApiResult<List<DTO.Material>>> GetMaterials()
        {
            try
            {
                return ApiResult(
                    await dbContext
                    .Materials
                    .Include(x => x.Measure)
                    .Select(x => new DTO.Material
                    {
                        Id = x.Id,
                        IsDeleted = x.IsDeleted,
                        Name = x.Name,
                        Description = x.Description,
                        MeasureId = x.MeasureId,
                        MeasureName = x.Measure.Name
                    })
                    .ToListAsync());
            }
            catch(Exception ex)
            {
                return ApiResult<List<DTO.Material>>(null, $"{ex.Message}\n{ex.InnerException?.Message}");
            }
        }

        [HttpPost]
        [Authorize(Roles = "ReferenceEdit")]
        public async Task<ApiResult<bool>> SaveMeasures([FromBody] DTO.Material material)
        {
            try
            {
                Material m;
                if (material.Id != default)
                {
                    m = await dbContext.Materials.
                        FirstOrDefaultAsync(x => x.Id == material.Id);
                    if (m == null)
                    {
                        return ApiResult(false, "Элемент справочника не найден!");
                    }
                }
                else
                {
                    m = new Material();
                    dbContext.Add(m);
                }

                m.Name = material.Name;
                m.MeasureId = material.MeasureId;

                await dbContext.SaveChangesAsync();
            }
            catch (Exception ex)
            {
                return ApiResult(false, ex.Message);
            }
            return ApiResult(true);
        }
    }
}