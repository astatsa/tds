using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.ChangeTracking;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace TDSServer.Services
{
    public class LastEntityChangesService
    {
        private Dictionary<string, DateTime> entityChanges = new Dictionary<string, DateTime>();

        public void EntityChanged(ChangeTracker changeTracker)
        {
            foreach(var e in changeTracker.Entries()
                .Where(
                    x =>
                    (x.State == EntityState.Modified
                    || x.State == EntityState.Added
                    || x.State == EntityState.Deleted)
                    && x.Metadata.ClrType != null)
                .Select(x => x.Metadata.ClrType.FullName)
                .Distinct())
            {
                entityChanges[e] = DateTime.Now;
            }
        }

        public DateTime GetEntityLastChangeDate<TModel>()
        {
            if(!entityChanges.TryGetValue(typeof(TModel).FullName, out DateTime res))
            {
                return DateTime.MinValue;
            }
            return res;
        }
    }
}
