using Mapster;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;
using Microsoft.CodeAnalysis.CSharp.Syntax;
using Microsoft.EntityFrameworkCore;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Runtime.CompilerServices;
using System.Threading.Tasks;
using TDSDTO;

namespace TDSServer.Controllers
{
    public abstract class BaseReferenceController<TModel, TDTO> : BaseTDSController where TModel : Models.BaseModel, new() 
        where TDTO : TDSDTO.BaseModel, new()
    {
        public BaseReferenceController(AppDbContext dbContext) : base(dbContext)
        {
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
        public virtual async Task<ApiResult<bool>> MarkUnmarkToRemove(int id)
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
                BeforeDelete(entity);
                await dbContext.SaveChangesAsync();
            }
            catch(Exception ex)
            {
                return ApiResult(false, $"{ex.Message}\n{ex.InnerException?.Message}");
            }
            return ApiResult(true);
        }

        protected virtual void BeforeDelete(TModel model)
        {

        }

        protected async Task<ApiResult<bool>> Save(TDTO dto)
        {
            try
            {
                TModel model;
                if (dto.Id != default)
                {
                    model = await dbContext
                        .Set<TModel>().
                        FirstOrDefaultAsync(x => x.Id == dto.Id);
                    if (model == null)
                    {
                        return new ApiResult<bool>(false, "Элемент справочника не найден!");
                    }
                }
                else
                {
                    model = new TModel();
                    dbContext.Add(model);
                }

                dto.Adapt(model);

                await dbContext.SaveChangesAsync();
            }
            catch (Exception ex)
            {
                return new ApiResult<bool>(false, $"{ex.Message}\n{ex.InnerException?.Message}");
            }
            return new ApiResult<bool>(true);
        }
    }
}
