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

namespace TDSServer.Controllers
{
    [Route("api/[controller]")]
    [ApiController]
    [Authorize]
    public class RefuelsController : ControllerBase
    {
        private AppDbContext dbContext;
        public RefuelsController(AppDbContext dbContext)
        {
            this.dbContext = dbContext;
        }

        [HttpPost]
        [Authorize(Roles = "MobileApp")]
        public async Task<ApiResult<bool>> AddRefuel([FromBody] Refuel refuel)
        {
            if (!int.TryParse(HttpContext.User.Identity.Name, out int userId))
            {
                return new ApiResult<bool>(false, "Не указан идентификатор пользователя!");
            }

            var driver = await dbContext
                .Employees
                .FirstOrDefaultAsync(x => x.UserId == userId);

            if(driver == null)
            {
                return new ApiResult<bool>(false, "Не удалось найти сотрудника!");
            }

            refuel.Date = DateTime.Now;
            refuel.Driver = driver;
            dbContext.Entry(refuel.GasStation).State = EntityState.Unchanged;
            dbContext.Refuels.Add(refuel);
            try
            {
                await dbContext.SaveChangesAsync();
            }
            catch(Exception ex)
            {
                return new ApiResult<bool>(false, ex.Message);
            }
            return new ApiResult<bool>(true);
        }
    }
}