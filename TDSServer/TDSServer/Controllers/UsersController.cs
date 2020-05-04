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
using DTO = TDSDTO.References;

namespace TDSServer.Controllers
{
    [Route("api/[controller]")]
    [ApiController]
    [Authorize]
    public class UsersController : BaseReferenceController<Models.User, DTO.User>
    {
        public UsersController(AppDbContext dbContext) : base(dbContext)
        {
        }

        [HttpGet]
        [Authorize(Roles = "ReferenceRead")]
        public async Task<ApiResult<List<DTO.User>>> GetUsers()
        {
            try
            {
                return ApiResult(
                    await dbContext.Users
                    .ProjectToType<DTO.User>()
                    .ToListAsync());
            }
            catch(Exception ex)
            {
                return ApiResult<List<DTO.User>>(null, $"{ex.Message}\n{ex.InnerException?.Message}");
            }
        }

        [HttpPost]
        [Authorize(Roles = "UserEdit")]
        public Task<ApiResult<bool>> SaveUsers([FromBody] DTO.User user)
            => Save(user);
    }
}