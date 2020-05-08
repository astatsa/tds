using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using Mapster;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;
using TDSServer.Models;
using TDSDTO;
using DTO = TDSDTO;
using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Query.Internal;
using System.Security.Cryptography.X509Certificates;

namespace TDSServer.Controllers
{
    [Route("api/[controller]")]
    [ApiController]
    [Authorize]
    public class CounterpartyRestsController : BaseTDSController
    {
        public CounterpartyRestsController(AppDbContext dbContext) : base(dbContext)
        {
        }

        [HttpGet("counterparty/{counterpartyId}")]
        [Authorize(Roles = "ReferenceRead")]
        public async Task<ApiResult<List<DTO.CounterpartyMaterialRest>>> GetCounterpartyRests(int counterpartyId)
        {
            try
            {
                return ApiResult(await dbContext.Materials
                    .GroupJoin(dbContext.CounterpartyMaterialRests.Where(x => x.CounterpartyId == counterpartyId), 
                        x => x.Id, x => x.MaterialId,
                        (x, y) => new { Material = x, Rests = y })
                    .SelectMany(x => x.Rests.DefaultIfEmpty(),
                        (x, rest) => new DTO.CounterpartyMaterialRest
                        {
                            CounterpartyId = counterpartyId,
                            MaterialId = rest == null && x.Material.IsDeleted ? 0 : x.Material.Id,
                            MaterialName = x.Material.Name,
                            Rest = rest == null ? 0 : rest.Rest
                        })
                    .Where(x => x.MaterialId > 0)
                    .ToListAsync());
            }
            catch(Exception ex)
            {
                return ApiResult<List<DTO.CounterpartyMaterialRest>>(null, $"{ex.Message}\n{ex.InnerException?.Message}");
            }
        }
    }
}