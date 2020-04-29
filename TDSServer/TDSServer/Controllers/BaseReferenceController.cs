using Mapster;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;
using Microsoft.CodeAnalysis.CSharp.Syntax;
using Microsoft.EntityFrameworkCore;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using TDSDTO;

namespace TDSServer.Controllers
{
    public abstract class BaseReferenceController<TModel, TDTO> : BaseTDSController where TModel : Models.BaseModel where TDTO : TDSDTO.BaseModel
    {
        protected AppDbContext dbContext;

        public BaseReferenceController(AppDbContext dbContext)
        {
            this.dbContext = dbContext;
        }

        [HttpGet("{id}")]
        [Authorize(Roles = "ReferenceRead")]
        public async virtual Task<ApiResult<TDTO>> GetEntity(int id)
        {
            try
            {
                return ApiResult<TDTO>(
                    await dbContext
                        .Set<TModel>()
                        .Where(x => x.Id == id)
                        .Select(x => x.Adapt<TDTO>())
                        .FirstOrDefaultAsync());
            }
            catch(Exception ex)
            {
                return ApiResult<TDTO>(null, $"{ex.Message}\n{ex.InnerException?.Message}");
            }
        }

        [HttpPost("delete/{id}")]
        [Authorize(Roles = "ReferenceEdit")]
        public async Task<ApiResult<bool>> MarkUnmarkToRemove(int id)
        {
            var entity = await dbContext
                .Set<TModel>()
                .FirstOrDefaultAsync(x => x.Id == id);

            if(entity == null)
            {
                return ApiResult(false, "Элемент не найден!");
            }

            entity.IsDeleted = !entity.IsDeleted;
            
            try
            {
                await dbContext.SaveChangesAsync();
            }
            catch(Exception ex)
            {
                return ApiResult(false, $"{ex.Message}\n{ex.InnerException?.Message}");
            }
            return ApiResult(true);
        }
    }
}
