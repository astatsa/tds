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
    public class MaterialsController : BaseReferenceController<Models.Material, DTO.Material>
    {
        public MaterialsController(AppDbContext dbContext) : base(dbContext)
        {
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
        public Task<ApiResult<bool>> SaveMeasures([FromBody] DTO.Material material)
            => Save(material);
    }
}