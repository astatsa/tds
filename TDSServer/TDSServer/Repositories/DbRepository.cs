using Mapster;
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

        public void AddMovements<TRegister, TDoc>(TDoc document, IEnumerable<TRegister> movements) where TDoc : DocumentBaseModel where TRegister : RegisterBaseModel
        {
            //Удаляем движения документа
            dbContext
                .Set<TRegister>()
                .RemoveRange(GetDocumentMovements<TRegister, TDoc>(document));

            //Записываем движения
            dbContext
                .Set<TRegister>()
                .AddRange(movements.Select(x =>
                {
                    x.RegistratorTypeId = document.DocumentTypeId;
                    x.RegistratorId = document.Id;
                    return x;
                }));
        }

        public void AddMovements<TRegister, TDoc>(TDoc document) where TDoc : DocumentBaseModel where TRegister : RegisterBaseModel
        {
            AddMovements<TRegister, TDoc>(document, new TRegister[] { document.Adapt<TRegister>() });
        }
    }
}
