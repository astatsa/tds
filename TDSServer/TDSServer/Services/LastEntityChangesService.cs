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

        public void EntityChanged(ICollection<string> changedEntities)
        {
            foreach(var e in changedEntities)
            {
                entityChanges[e] = DateTime.UtcNow;
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
