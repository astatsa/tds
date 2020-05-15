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
                    .Include(x => x.UserRoles)
                    .Select(x => new DTO.User
                    {
                        Id = x.Id,
                        IsDeleted = x.IsDeleted,
                        FullName = x.FullName,
                        Username = x.Username,
                        RoleId = x.UserRoles.Select(r => r.RoleId).FirstOrDefault()
                    })
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
        {
            dbContext.Database.BeginTransaction();
            try
            {
                var res = Save(user);
                if (!res.Result)
                    return res;

                dbContext.UserRoles.RemoveRange()
                
                dbContext.Database.CommitTransaction();
            }
            catch(Exception ex)
            {
                dbContext.Database.RollbackTransaction();
                return ApiResult(false, $"{ex.Message}\n{ex.InnerException?.Message}");
            }
            
        }
    }
}