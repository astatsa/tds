using System;
using System.Collections.Generic;
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
    public class PositionsController : ControllerBase
    {
        private readonly AppDbContext dbContext;

        public PositionsController(AppDbContext dbContext)
        {
            this.dbContext = dbContext;
        }

        [HttpGet]
        [Authorize(Roles = "PositionRead")]
        public Task<List<DTO.Position>> GetPositions() =>
            dbContext.Positions
            .Select(x => new DTO.Position
            {
                Id = x.Id,
                Name = x.Name,
                Description = x.Description,
                IsDeleted = x.IsDeleted
            })
            .ToListAsync();
    }
}