using Mapster;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Runtime.CompilerServices;
using System.Threading.Tasks;
using TDSDTO;
using TDSServer.Models;
using TDSServer.Repositories;
using DTO = TDSDTO.Documents;

namespace TDSServer.Controllers
{
    [Route("api/[controller]")]
    [ApiController]
    [Authorize]
    public class CounterpartyRestCorrectionsController : BaseReferenceController<CounterpartyRestCorrection, DTO.CounterpartyRestCorrection>
    {
        private readonly DbRepository repository;

        public CounterpartyRestCorrectionsController(AppDbContext dbContext, DbRepository repository) : base(dbContext)
        {
            this.repository = repository;
        }

        [HttpGet("/counterparty/{id}")]
        [Authorize(Roles = "ReferenceRead")]
        public async Task<ApiResult<List<DTO.CounterpartyRestCorrection>>> GetCounterpartyCorrections(int counterpartyId)
        {
            try
            {
                return ApiResult(await dbContext.CounterpartyRestCorrections
                    .Where(x => x.CounterpartyId == counterpartyId)
                    .ProjectToType<DTO.CounterpartyRestCorrection>()
                    .ToListAsync());
            }
            catch(Exception ex)
            {
                return ApiResult<List<DTO.CounterpartyRestCorrection>>(null, $"{ex.Message}\n{ex.InnerException?.Message}");
            }
        }

        [HttpPost]
        [Authorize(Roles = "ReferenceEdit")]
        public async Task<ApiResult<bool>> SaveCorrection([FromBody] DTO.CounterpartyRestCorrection document)
        {
            try
            {
                var rests = await dbContext.CounterpartyMaterialRests
                    .Where(x => x.CounterpartyId == document.CounterpartyId)
                    .Select(x => new { x.MaterialId, x.Rest })
                    .ToListAsync();

                int.TryParse(HttpContext.User.Identity.Name, out int userId);
                userId = await dbContext.Users
                    .Where(x => x.Id == userId)
                    .Select(x => x.Id)
                    .FirstOrDefaultAsync();

                var doc = new CounterpartyRestCorrection
                {
                    CounterpartyId = document.CounterpartyId,
                    Date = document.Date,
                    IsDeleted = document.IsDeleted,
                    UserId = userId,
                    MaterialCorrections =
                            document
                            .MaterialCorrections
                            .GroupJoin(rests, x => x.MaterialId, x => x.MaterialId,
                                (x, y) => new { Corr = x, Rests = y })
                            .SelectMany(x => x.Rests.DefaultIfEmpty(),
                                (x, rest) =>
                                new
                                {
                                    x.Corr.MaterialId,
                                    Correction = x.Corr.Correction - (rest == null ? 0 : rest.Rest)
                                })
                            .Adapt<List<CounterpartyRestCorrectionMaterial>>()
                };

                dbContext.Database.BeginTransaction();
                dbContext.CounterpartyRestCorrections.Add(doc);
                await dbContext.SaveChangesAsync();

                repository.AddMovements<CounterpartyMaterialMvt, CounterpartyRestCorrection>(doc, 
                    doc.MaterialCorrections
                    .Select(x => new CounterpartyMaterialMvt
                    {
                        CounterpartyId = doc.CounterpartyId,
                        Date = doc.Date,
                        MaterialId = x.MaterialId,
                        Quantity = x.Correction
                    }));

                //Корректировка остатков
                foreach(var r in doc.MaterialCorrections
                    .GroupJoin(rests, x => x.MaterialId, x => x.MaterialId, (x, y) => new { Corr = x, Rests = y })
                    .SelectMany(x => x.Rests.DefaultIfEmpty(),
                        (x, y) => new
                        {
                            State = y == null ? EntityState.Added : EntityState.Unchanged,
                            Entity = new Models.CounterpartyMaterialRest
                            {
                                CounterpartyId = doc.CounterpartyId,
                                MaterialId = x.Corr.MaterialId,
                                Rest = y == null ? x.Corr.Correction : y.Rest + x.Corr.Correction
                            }
                        }))
                {
                    var entry = dbContext.Attach(r.Entity);
                    entry.State = r.State;
                    if (r.State == EntityState.Unchanged)
                        entry.Property(x => x.Rest).IsModified = true;
                }

                await dbContext.SaveChangesAsync();
                dbContext.Database.CommitTransaction();
            }
            catch(Exception ex)
            {
                dbContext.Database.RollbackTransaction();
                return ApiResult(false, $"{ex.Message}\n{ex.InnerException?.Message}");
            }

            return ApiResult(true);
        }
    }
}