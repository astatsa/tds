using MobileApp.Models;
using Newtonsoft.Json;
using SQLite;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

namespace MobileApp.Repositories
{
    class DbRepository : IDisposable
    {
        private readonly SQLiteConnection db;
        public DbRepository(string path)
        {
            db = new SQLiteConnection(path);
            db.CreateTable<Models.FailedMethodCall>();
        }

        public void SaveFailedMethodCall(string methodName, params object[] parameters)
        {
            db.Insert(
                new FailedMethodCall
                {
                    MethodName = methodName,
                    Parameters = JsonConvert.SerializeObject(
                        parameters
                        .Select(p => new Parameter(p)))
                });
        }

        public List<(int id, string MethodName, Parameter[] Parameters)> GetFailedMethods()
            => db.Table<FailedMethodCall>()
                .OrderBy(x => x.Id)
                .Select(x =>
                {
                    Parameter[] parameters = null;
                    try
                    {
                        parameters = JsonConvert.DeserializeObject<Parameter[]>(x.Parameters);
                    }
                    catch
                    {
                        return (0, null, null);
                    }
                    return (x.Id, x.MethodName, parameters);
                })
                .Where(x => x.Id != 0)
                .ToList();

        public void DeleteFailedMethod(int id)
            => db.Delete<FailedMethodCall>(id);

        public void Dispose()
        {
            db.Dispose();
        }
    }
}
