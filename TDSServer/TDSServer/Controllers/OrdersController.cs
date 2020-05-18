using System;
using System.Collections.Generic;
using System.Linq;
using System.Linq.Expressions;
using System.Security.Cryptography.X509Certificates;
using System.Threading.Tasks;
using Mapster;
using Mapster.Utils;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;
using TDSDTO;
using TDSDTO.Filter;
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
        public async Task<ApiResult<List<DTO.Order>>> GetOrdersByEmployee(int employeeId)
        {
            try
            {
                return ApiResult(await GetAllOrders()
                    .Where(x => x.DriverId == employeeId)
                    .ToListAsync());
            }
            catch(Exception ex)
            {
                return ApiResult<List<DTO.Order>>(null, $"{ex.Message}\n{ex.InnerException?.Message}");
            }
        }

        [HttpGet("current")]
        [Authorize(Roles = "OrderRead")]
        public async Task<ApiResult<DTO.Order>> GetDriverCurrentOrder()
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
                return ApiResult<DTO.Order>(
                    order == null ? null :
                    new DTO.Order
                    {
                        Id = order.Id,
                        Date = order.Date,
                        Number = order.Number,
                        DateCreate = order.DateCreate,
                        Volume = order.Volume,
                        CustomerId = order.CustomerId,
                        CustomerName = order.Customer.Name,
                        SupplierId = order.SupplierId,
                        SupplierName = order.Supplier.Name,
                        MaterialId = order.MaterialId,
                        MaterialName = order.Material.Name,
                        DriverId = order.DriverId,
                        DriverName = order.Driver.Name,
                        IsDeleted = order.IsDeleted,
                        OrderStateId = order.OrderStateId,
                        OrderStateName = order.OrderState.FullName,
                        OrderState = (DTO.OrderStates)order.OrderState.Name
                    });
            }
            return ApiResult<DTO.Order>(default, "Не удалось определить идентификатор пользователя!");
        }

        [HttpPost("{id}")]
        [Authorize(Roles = "MobileApp")]
        public async Task<ApiResult<bool>> SetStateAndWeight(int id, [FromBody] OrderWeightAndState data)
        {
            try
            {
                var order = await dbContext
                    .Orders
                    .FirstOrDefaultAsync(x => x.Id == id);
                if (order == null)
                    return ApiResult(false, $"Заявка с id={id} не найдена!");
                OrderStates modelOrderState = (OrderStates)data.OrderState;
                order.OrderState = await dbContext.OrderStates.FirstOrDefaultAsync(x => x.Name == modelOrderState);
                if (order.OrderState.Name == OrderStates.Loaded)
                {
                    order.LoadedVolume = data.Weight;
                    order.LoadedDate = DateTime.Now;
                }
                else if (order.OrderState.Name == OrderStates.Completed)
                {
                    order.UnloadedVolume = data.Weight;
                    order.UnloadedDate = DateTime.Now;
                }

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
                await AddMovementsAsync(order);

                return ApiResult(true);
            }
            catch(Exception ex)
            {
                return ApiResult(false, $"{ex.Message}\n{ex.InnerException?.Message}");
            }
        }

        [HttpGet]
        [Authorize(Roles = "OrderRead")]
        public async Task<ApiResult<List<DTO.Order>>> GetOrders()
        {
            try
            {
                return ApiResult(await 
                    GetAllOrders()
                    .OrderBy(x => x.Number)
                    .AsNoTracking()
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
                dbContext.Database.BeginTransaction();

                order.Adapt(orderModel);
                if(orderModel.Number == default)
                {
                    orderModel.Number = (await dbContext
                        .Orders
                        .Select(x => (int?)x.Number)
                        .MaxAsync() ?? 0) + 1;
                }
                if(order.Date == null)
                {
                    orderModel.Date = DateTime.Now;
                }

                orderModel.OrderState = 
                    orderModel.OrderStateId != default ? 
                    dbContext.OrderStates.FirstOrDefault(x => x.Id == orderModel.OrderStateId) :
                    dbContext.OrderStates.FirstOrDefault(x => x.Name == OrderStates.New);

                await dbContext.SaveChangesAsync();

                //Запись движений
                await AddMovementsAsync(orderModel);

                dbContext.Database.CommitTransaction();
            }
            catch(Exception ex)
            {
                dbContext.Database.RollbackTransaction();
                return ApiResult(false, $"{ex.Message}\n{ex.InnerException?.Message}");
            }
            return ApiResult(true);
        }

        protected override Task BeforeDeleteAsync(Order model)
        {
            return AddMovementsAsync(model);
        }

        private async Task AddMovementsAsync(Order model)
        {
            //Удаление движений документа
            dbRepository.DeleteCounterpartyMaterialMovements(model);
            await dbContext.SaveChangesAsync();
            //Запись движений

            var stateIdForMove = dbContext.OrderStates.Where(x => x.Name == OrderStates.Loaded).Select(x => x.Id).FirstOrDefault();
            if (model.OrderStateId == stateIdForMove  && !model.IsDeleted)
            {
                dbRepository.AddCounterpartyMaterialMovements(model, new[]
                        {
                            new CounterpartyMaterialMvt
                            {
                                CounterpartyId = model.SupplierId,
                                Date = model.Date,
                                IsComing = false,
                                MaterialId = model.MaterialId,
                                Quantity = -model.LoadedVolume
                            }
                        });
            }
            await dbContext.SaveChangesAsync();
        }

        private IQueryable<DTO.Order> GetAllOrders()
            => dbContext
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
                        OrderStateId = x.OrderStateId,
                        OrderStateName = x.OrderState.FullName,
                        OrderState = (DTO.OrderStates)x.OrderState.Name,
                        LoadedDate = x.LoadedDate,
                        LoadedVolume = x.LoadedVolume,
                        UnloadedDate = x.UnloadedDate,
                        UnloadedVolume = x.UnloadedVolume
                    });
    }
}