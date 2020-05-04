using Mapster;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using TDSDTO;
using TDSServer.Models;
using DTO = TDSDTO.Documents;

namespace TDSServer.Controllers
{
    [Route("api/[controller]")]
    [ApiController]
    [Authorize]
    public class RefuelsController : BaseReferenceController<Models.Refuel, DTO.Refuel>
    {
        public RefuelsController(AppDbContext dbContext) : base(dbContext)
        {
        }

        [HttpPost]
        [Authorize(Roles = "MobileApp")]
        public async Task<ApiResult<bool>> AddRefuel([FromBody] Refuel refuel)
        {
            if (!int.TryParse(HttpContext.User.Identity.Name, out int userId))
            {
                return ApiResult(false, "Не указан идентификатор пользователя!");
            }

            var driver = await dbContext
                .Employees
                .FirstOrDefaultAsync(x => x.UserId == userId);

            if (driver == null)
            {
                return ApiResult(false, "Не удалось найти сотрудника!");
            }

            refuel.Date = DateTime.Now;
            refuel.Driver = driver;
            dbContext.Entry(refuel.GasStation).State = EntityState.Unchanged;
            dbContext.Refuels.Add(refuel);
            try
            {
                await dbContext.SaveChangesAsync();
            }
            catch (Exception ex)
            {
                return ApiResult(false, ex.Message);
            }
            return ApiResult(true);
        }

        [HttpGet]
        [Authorize(Roles = "RefuelRead")]
        public async Task<ApiResult<List<DTO.Refuel>>> GetRefuels()
        {
            try
            {
                return ApiResult(
                    await dbContext
                    .Refuels
                    .Include(x => x.GasStation)
                    .Include(x => x.Driver)
                    .OrderBy(x => x.Date)
                    .ProjectToType<DTO.Refuel>()
                    .ToListAsync());
            }
            catch(Exception ex)
            {
                return ApiResult<List<DTO.Refuel>>(null, $"{ex.Message}\n{ex.InnerException?.Message}");
            }
        }
    }
}