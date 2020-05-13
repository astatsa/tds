using MobileApp.Repositories;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace MobileApp.Services
{
    class RepeatFailedMethodService
    {
        private readonly ApiRepository apiRepository;
        private readonly DbRepository dbRepository;

        public RepeatFailedMethodService(ApiRepository apiRepository, DbRepository dbRepository)
        {
            this.apiRepository = apiRepository;
            this.dbRepository = dbRepository;
        }

        public async void StartTryMethodsCall()
        {
            while (true)
            {
                await Task.Delay(5000);
                var methods = dbRepository.GetFailedMethods();
                var type = apiRepository.GetType();
                foreach (var m in methods)
                {
                    var method = type.GetMethod(m.MethodName);
                    //try
                    {
                        var res = method.Invoke(apiRepository,
                            m.Parameters
                            .Select(
                                x =>
                                {
                                    var t = Type.GetType(x.TypeName);
                                    if(t.IsEnum)
                                    {
                                        return Enum.ToObject(t, x.Value);
                                    }
                                    else
                                        return Convert.ChangeType(x.Value, t);
                                })
                            .ToArray());
                        if (res is Task<bool> taskRes)
                        {
                            try
                            {
                                if (await taskRes)
                                {
                                    dbRepository.DeleteFailedMethod(m.id);
                                }
                            }
                            catch
                            {

                            }
                        }
                    }
                    /*catch
                    {

                    }*/
                }
            }
        }

        
    }
}
