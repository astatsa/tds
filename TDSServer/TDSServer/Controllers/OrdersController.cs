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
using TDSServer.Repositories;
using TDSServer.Services;
using DTO = TDSDTO.Documents;

namespace TDSServer.Controllers
{
    [Route("api/[controller]")]
    [ApiController]
    [Authorize]
    public class OrdersController : BaseReferenceController<Models.Order, DTO.Order>
    {
        private readonly DbRepository dbRepository;
        public OrdersController(AppDbContext dbContext, DbRepository dbRepository) : base(dbContext)
        {
            this.dbRepository = dbRepository;
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
            return ApiResult<Order>(default, "Не удалось определить идентификатор пользователя!");
        }

        [HttpPost("{id}")]
        [Authorize(Roles = "MobileApp")]
        public async Task<ApiResult<bool>> SetState(int id, [FromBody] OrderStates state)
        {
            var order = await dbContext
                .Orders
                .FirstOrDefaultAsync(x => x.Id == id);
            if (order == null)
                return ApiResult<bool>(false, $"Заявка с id={id} не найдена!");

            order.OrderState = await dbContext.OrderStates.FirstAsync(x => x.Name == state);

            //await dbContext.SaveChangesAsync();

            int.TryParse(HttpContext.User.Identity.Name, out int userId);
            dbContext.OrderStateHistories.Add(new OrderStateHistory
            {
                Date = DateTime.Now,
                Order = order,
                OrderState = order.OrderState,
                UserId = userId
            });

            //Изменение остатков материала
            AddMovements(order);

            await dbContext.SaveChangesAsync();

            return ApiResult<bool>(true);
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
                    .OrderBy(x => x.Date)
                    .ThenBy(x => x.Number)
                    .ThenBy(x => x.Id)
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

        [HttpPost]
        [Authorize(Roles = "OrderEdit")]
        public async Task<ApiResult<bool>> SaveOrder([FromBody] DTO.Order order)
        {
            if(order == null)
                return ApiResult(false, "Нет данных для записи!");

            Order orderModel;
            if(order.Id == default)
            {
                orderModel = new Order();
                dbContext.Add(orderModel);
            }
            else
            {
                orderModel = await dbContext
                    .Orders
                    .FirstOrDefaultAsync(x => x.Id == order.Id);
                if (orderModel == null)
                    return ApiResult(false, "Документ не найден!");
            }

            try
            {
                order.Adapt(orderModel);
                if(orderModel.Number == default)
                {
                    orderModel.Number = await dbContext.Orders.MaxAsync(x => x.Number) + 1;
                }
                if(orderModel.OrderState == null)
                {
                    orderModel.OrderState = dbContext
                        .OrderStates
                        .FirstOrDefault(x => x.Name == OrderStates.New);
                }

                //Запись движений
                AddMovements(orderModel);

                await dbContext.SaveChangesAsync();
            }
            catch(Exception ex)
            {
                return ApiResult(false, $"{ex.Message}\n{ex.InnerException.Message}");
            }
            return ApiResult(true);
        }

        protected override void BeforeDelete(Order model)
        {
            base.BeforeDelete(model);
            AddMovements(model);
        }

        private void AddMovements(Order model)
        {
            //Удаление движений документа
            dbRepository.DeleteMovements<CounterpartyMaterialMvt, Order>(model);

            //Запись движений
            if (model.OrderState?.Name == OrderStates.Completed && !model.IsDeleted)
            {
                dbRepository.AddMovements<CounterpartyMaterialMvt, Order>(model,
                        new[]
                        {
                            new CounterpartyMaterialMvt
                            {
                                CounterpartyId = model.SupplierId,
                                Date = model.Date,
                                IsComing = false,
                                MaterialId = model.MaterialId,
                                Quantity = model.Volume
                            }
                        });
            }
        }
    }
}