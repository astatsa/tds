using System;
using System.Collections.Generic;
using System.Data.Common;
using System.Linq;
using System.Threading.Tasks;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;
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
                TypeId = x.TypeId
            })
            .ToListAsync();
    }
}