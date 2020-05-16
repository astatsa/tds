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
using TDSServer.Models;
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
        public async Task<ApiResult<bool>> SaveUsers([FromBody] DTO.User user)
        {
            dbContext.Database.BeginTransaction();
            try
            {
                User model;
                if (user.Id != default)
                {
                    model = await dbContext
                        .Users
                        .FirstOrDefaultAsync(x => x.Id == user.Id);
                    if (model == null)
                    {
                        return new ApiResult<bool>(false, "Элемент справочника не найден!");
                    }

                    //Удаляем все роли пользователя
                    dbContext.RemoveRange(
                        dbContext.UserRoles.Where(x => x.UserId == user.Id));
                }
                else
                {
                    model = new User();
                    dbContext.Add(model);
                }

                var pass = model.PasswordHash;
                user.Adapt(model);
                if (user.PasswordHash == null)
                    model.PasswordHash = pass;
                await dbContext.SaveChangesAsync();

                dbContext.UserRoles.Add(
                    new UserRole
                    {
                        RoleId = user.RoleId,
                        UserId = model.Id
                    });

                await dbContext.SaveChangesAsync();
                dbContext.Database.CommitTransaction();
            }
            catch (Exception ex)
            {
                dbContext.Database.RollbackTransaction();
                return new ApiResult<bool>(false, $"{ex.Message}\n{ex.InnerException?.Message}");
            }
            return new ApiResult<bool>(true);            
        }
    }
}