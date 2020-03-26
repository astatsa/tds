using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;

namespace TDSServer.Controllers
{
    [Route("api/[controller]")]
    [ApiController]
    [Authorize]
    public class OrderController : ControllerBase
    {
        [HttpGet("{id}")]
        [Authorize(Roles = "OrderRead")]
        public IActionResult GetOrder(int id)
        {
            return Ok(new { OrderId = id, Material = "Sand", Volume = "10" });
        }
    }
}