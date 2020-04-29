using System;
using System.Collections.Generic;
using System.Data.Common;
using System.Linq;
using System.Linq.Expressions;
using System.Threading.Tasks;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;
using TDSDTO;
using TDSDTO.Filter;
using TDSServer.Models;
using DTO = TDSDTO.References;


namespace TDSServer.Controllers
{
    [Route("api/[controller]")]
    [ApiController]
    [Authorize]
    public class CounterpartiesController : BaseReferenceController<Models.Counterparty, DTO.Counterparty>
    {
        public CounterpartiesController(AppDbContext dbContext) : base(dbContext)
        {
        }

        [Authorize(Roles = "ReferenceRead")]
        [HttpGet]
        public async Task<ApiResult<List<DTO.Counterparty>>> GetCounterparties([FromBody] FilterConditionGroup filter = null)
        {
            try
            {
                var predicate = filter?.GetPredicate<DTO.Counterparty>() ?? (x => true);
                var res = await dbContext.Counterparties
                    .Include(x => x.Type)
                    .Select(x => new DTO.Counterparty
                    {
                        Name = x.Name,
                        Address = x.Address,
                        Description = x.Description,
                        TypeName = x.Type.Name == Models.CounterpartyTypes.Customer ? "Покупатель" : "Поставщик",
                        Id = x.Id,
                        IsDeleted = x.IsDeleted,
                        IsSupplier = x.Type.Name == Models.CounterpartyTypes.Supplier
                    })
                    .ToListAsync();
                
                return ApiResult(res.Where(predicate).ToList());
            }
            catch(Exception ex)
            {
                return ApiResult<List<DTO.Counterparty>>(null, $"{ex.Message}\n{ex.InnerException?.Message}");
            }
        }

        [HttpPost]
        [Authorize(Roles = "ReferenceEdit")]
        public async Task<ApiResult<bool>> SaveCounterparty([FromBody] DTO.Counterparty counterparty)
        {
            try
            {
                Models.Counterparty cp;
                if (counterparty.Id != default)
                {
                    cp = await dbContext.Counterparties.
                        FirstOrDefaultAsync(x => x.Id == counterparty.Id);
                    if (cp == null)
                    {
                        return new ApiResult<bool>(false, "Элемент справочника не найден!");
                    }
                }
                else
                {
                    cp = new Models.Counterparty();
                    dbContext.Add(cp);
                }

                var cpType = counterparty.IsSupplier ? CounterpartyTypes.Supplier : CounterpartyTypes.Customer;
                cp.Name = counterparty.Name;
                cp.Address = counterparty.Address;
                cp.Description = counterparty.Description;
                cp.Type = dbContext.CounterpartyTypes.
                    FirstOrDefault(x => x.Name == cpType);

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