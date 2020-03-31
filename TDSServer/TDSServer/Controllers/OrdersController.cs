using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;
using TDSServer.Models;

namespace TDSServer.Controllers
{
    [Route("api/[controller]")]
    [ApiController]
    [Authorize]
    public class OrdersController : ControllerBase
    {
        private AppDbContext dbContext;
        public OrdersController(AppDbContext dbContext)
        {
            this.dbContext = dbContext;
        }

        [HttpGet("employee/{employeeId}")]
        [Authorize(Roles = "OrderRead")]
        public async Task<ApiResult<IEnumerable<Order>>> GetOrdersByEmployee(int employeeId) => 
            new ApiResult<IEnumerable<Order>>
            {
                Result = await dbContext.Orders
                .Where(x => x.DriverId == employeeId)
                .Include(x => x.Material)
                .Include(x => x.Supplier)
                .Include(x => x.Customer)
                .ToListAsync()
            };

        [HttpGet("current")]
        [Authorize(Roles = "OrderRead")]
        public async Task<ApiResult<Order>> GetDriverCurrentOrder()
        {
            if(int.TryParse(HttpContext.User.Identity.Name, out int userId))
            {
                var order = await dbContext
                    .Orders
                    .Include(x => x.OrderState)
                    .Include(x => x.Driver)
                    .Include(x => x.Supplier)
                    .Include(x => x.Customer)
                    .Include(x => x.Material)
                    .ThenInclude(x => x.Measure)
                    .Where(x => x.OrderState.Name != OrderStates.Completed && x.Driver != null && x.Driver.UserId == userId)
                    .OrderByDescending(x => x.Number)
                    .FirstOrDefaultAsync();
                return new ApiResult<Order>(order);
            }
            return new ApiResult<Order>(default, "Не удалось определить идентификатор пользователя!");
        }
    }
}