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

        [HttpGet("{id}")]
        [Authorize(Roles = "OrderRead")]
        public IActionResult GetOrder(int id)
        {
            return Ok(new { OrderId = id, Material = "Sand", Volume = "10" });
        }

        [HttpGet("employee/{employeeId}")]
        [Authorize(Roles = "OrderRead")]
        public async Task<IEnumerable<Order>> GetOrdersByEmployee(int employeeId) => 
            await dbContext.Orders
                .Where(x => x.DriverId == employeeId)
                .Include(x => x.Material)
                .Include(x => x.Supplier)
                .Include(x => x.Customer)
                .ToListAsync();
    }
}