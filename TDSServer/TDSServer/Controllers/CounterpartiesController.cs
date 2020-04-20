using System;
using System.Collections.Generic;
using System.Data.Common;
using System.Linq;
using System.Threading.Tasks;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;
using TDSServer.Models;
using DTO = TDSDTO.References;


namespace TDSServer.Controllers
{
    [Route("api/[controller]")]
    [ApiController]
    [Authorize]
    public class CounterpartiesController : ControllerBase
    {
        private readonly AppDbContext dbContext;

        public CounterpartiesController(AppDbContext dbContext)
        {
            this.dbContext = dbContext;
        }

        [Authorize(Roles = "CounterpartyRead")]
        [HttpGet]
        public Task<List<DTO.Counterparty>> GetCounterparties() =>
            dbContext.Counterparties
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

        [HttpPost]
        public async Task<ApiResult<bool>> SaveCounterparty([FromBody] DTO.Counterparty counterparty)
        {
            var cp = await dbContext.Counterparties
                .Where(x => x.Id == counterparty.Id)
                .FirstOrDefaultAsync();

            cp.Name = counterparty.Name;
            cp.Address = counterparty.Address;

            await dbContext.SaveChangesAsync();
            return new ApiResult<bool>(true);
        }
    }
}