using Mapster;
using Microsoft.EntityFrameworkCore;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using TDSServer.Models;

namespace TDSServer.Repositories
{
    public class DbRepository
    {
        private readonly AppDbContext dbContext;

        public DbRepository(AppDbContext dbContext)
        {
            this.dbContext = dbContext;
        }

        public IQueryable<TRegister> GetDocumentMovements<TRegister, TDoc>(TDoc document) where TDoc : DocumentBaseModel where TRegister : RegisterBaseModel
            => dbContext
            .Set<TRegister>()
            .Where(x => x.RegistratorTypeId == document.DocumentTypeId && x.RegistratorId == document.Id);

        public void DeleteMovements<TRegister, TDoc>(TDoc document, Action<IEnumerable<TRegister>> afterDelete = null) where TDoc : DocumentBaseModel where TRegister : RegisterBaseModel
        {
            var movements = GetDocumentMovements<TRegister, TDoc>(document);
            dbContext
                .Set<TRegister>()
                .RemoveRange(movements);

            afterDelete?.Invoke(movements.AsEnumerable());
        }

        public void AddMovements<TRegister, TDoc>(TDoc document, IEnumerable<TRegister> movements) where TDoc : DocumentBaseModel where TRegister : RegisterBaseModel
            => dbContext
            .Set<TRegister>()
            .AddRange(movements.Select(x =>
            {
                x.RegistratorTypeId = document.DocumentTypeId;
                x.RegistratorId = document.Id;
                return x;
            }));

        public void AddMovements<TRegister, TDoc>(TDoc document) where TDoc : DocumentBaseModel where TRegister : RegisterBaseModel
            => AddMovements<TRegister, TDoc>(document, new TRegister[] { document.Adapt<TRegister>() });

        public void DeleteCounterpartyMaterialMovements<TDoc>(TDoc document) where TDoc : DocumentBaseModel
        {
            DeleteMovements<CounterpartyMaterialMvt, TDoc>(document,
                m =>
                {
                    foreach(var rest in dbContext.CounterpartyMaterialRests
                        .AsNoTracking()
                        .Join(m, x => new { x.CounterpartyId, x.MaterialId }, x => new { x.CounterpartyId, x.MaterialId },
                        (x, y) => new CounterpartyMaterialRest
                        {
                            CounterpartyId = x.CounterpartyId,
                            MaterialId = x.MaterialId,
                            Rest = x.Rest - y.Quantity
                        }))
                    {
                        dbContext.Attach(rest).Property(x => x.Rest).IsModified = true;
                    }
                });
        }

        public void AddCounterpartyMaterialMovements<TDoc>(TDoc document, IEnumerable<CounterpartyMaterialMvt> movements) where TDoc : DocumentBaseModel
        {
            AddMovements<CounterpartyMaterialMvt, TDoc>(document, movements);

            foreach (var r in movements
                    .GroupJoin(dbContext.CounterpartyMaterialRests.AsNoTracking(), 
                        x => new { x.CounterpartyId, x.MaterialId }, 
                        x => new { x.CounterpartyId, x.MaterialId },
                        (x, y) => new { Mvt = x, Rests = y })
                    .SelectMany(x => x.Rests.DefaultIfEmpty(),
                        (x, y) => new
                        {
                            State = y == null ? EntityState.Added : EntityState.Unchanged,
                            Entity = new Models.CounterpartyMaterialRest
                            {
                                CounterpartyId = x.Mvt.CounterpartyId,
                                MaterialId = x.Mvt.MaterialId,
                                Rest = y == null ? x.Mvt.Quantity : y.Rest + x.Mvt.Quantity
                            }
                        }))
            {
                var entry = dbContext.Attach(r.Entity);
                entry.State = r.State;
                if (r.State == EntityState.Unchanged)
                    entry.Property(x => x.Rest).IsModified = true;
            }
        }
    }
}
