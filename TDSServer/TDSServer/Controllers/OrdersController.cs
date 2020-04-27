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
using TDSServer.Services;
using DTO = TDSDTO.Documents;

namespace TDSServer.Controllers
{
    [Route("api/[controller]")]
    [ApiController]
    [Authorize]
    public class OrdersController : BaseTDSController
    {
        private readonly AppDbContext dbContext;
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
                    .OrderBy(x => x.Number)
                    .FirstOrDefaultAsync();
                return new ApiResult<Order>(order);
            }
            return new ApiResult<Order>(default, "Не удалось определить идентификатор пользователя!");
        }

        [HttpPost("{id}")]
        [Authorize(Roles = "MobileApp")]
        public async Task<ApiResult<bool>> SetState(int id, [FromBody] OrderStates state)
        {
            var order = await dbContext
                .Orders
                .FirstOrDefaultAsync(x => x.Id == id);
            if (order == null)
                return new ApiResult<bool>(false, $"Заявка с id={id} не найдена!");

            order.OrderState = await dbContext.OrderStates.FirstAsync(x => x.Name == state);

            await dbContext.SaveChangesAsync();

            int.TryParse(HttpContext.User.Identity.Name, out int userId);
            dbContext.OrderStateHistories.Add(new OrderStateHistory
            {
                Date = DateTime.Now,
                Order = order,
                OrderState = order.OrderState,
                UserId = userId
            });
            await dbContext.SaveChangesAsync();

            return new ApiResult<bool>(true);
        }

        [HttpGet]
        [Authorize(Roles = "OrderRead")]
        public async Task<ApiResult<List<DTO.Order>>> GetOrders()
        {
            try
            {
                return ApiResult(await 
                    dbContext
                    .Orders
                    .Include(x => x.Customer)
                    .Include(x => x.Supplier)
                    .Include(x => x.Material)
                    .Include(x => x.OrderState)
                    .Include(x => x.Driver)
                    .Select(x => new DTO.Order
                    {
                        Id = x.Id,
                        Date = x.Date,
                        Number = x.Number,
                        DateCreate = x.DateCreate,
                        Volume = x.Volume,
                        CustomerId = x.CustomerId,
                        CustomerName = x.Customer.Name,
                        SupplierId = x.SupplierId,
                        SupplierName = x.Supplier.Name,
                        MaterialId = x.MaterialId,
                        MaterialName = x.Material.Name,
                        DriverId = x.DriverId,
                        DriverName = x.Driver.Name,
                        IsDeleted = x.IsDeleted,
                        OrderStateId = x.OrderState.Id,
                        OrderStateName = x.OrderState.FullName
                    })
                    .ToListAsync());
            }
            catch(Exception ex)
            {
                return ApiResult<List<DTO.Order>>(null, $"{ex.Message}\n{ex.InnerException?.Message}");
            }
        }
    }
}