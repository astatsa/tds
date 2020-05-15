using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using Mapster;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Query.Internal;
using TDSDTO;
using TDSServer.Models;
using DTO = TDSDTO.References;

namespace TDSServer.Controllers
{
    [Route("api/[controller]")]
    [ApiController]
    public class RolesController : BaseReferenceController<Role, DTO.Role>
    {
        public RolesController(AppDbContext dbContext) : base(dbContext)
        {
        }

        [HttpGet]
        [Authorize(Roles = "UserEdit")]
        public async Task<ApiResult<List<DTO.Role>>> GetRoles()
        {
            try
            {
                return ApiResult(
                    await dbContext
                    .Roles
                    .ProjectToType<DTO.Role>()
                    .ToListAsync()
                    );
            }
            catch(Exception ex)
            {
                return ApiResult<List<DTO.Role>>(null, $"{ex.Message}\n{ex.InnerException?.Message}");
            }
        }
    }
}