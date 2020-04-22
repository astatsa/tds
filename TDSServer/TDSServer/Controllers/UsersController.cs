using System;
using System.Collections.Generic;
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
    public class UsersController : ControllerBase
    {
        private readonly AppDbContext dbContext;

        public UsersController(AppDbContext dbContext)
        {
            this.dbContext = dbContext;
        }

        [HttpGet]
        [Authorize(Roles = "ReferenceRead")]
        public Task<List<DTO.User>> GetUsers() =>
            dbContext.Users
            .Select(x => new DTO.User
            {
                Username = x.Username,
                FullName = x.FullName,
                Id = x.Id,
                IsDeleted = x.IsDeleted
            })
            .ToListAsync();
    }
}