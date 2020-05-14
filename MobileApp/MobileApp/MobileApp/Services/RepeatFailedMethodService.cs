using MobileApp.Repositories;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Runtime.CompilerServices;
using System.Text;
using System.Threading;
using System.Threading.Tasks;

namespace MobileApp.Services
{
    class RepeatFailedMethodService
    {
        private readonly ApiRepository apiRepository;
        private readonly DbRepository dbRepository;
        private bool alive;
        private bool isRepeating = false;
        private object syncObj = new object();

        public RepeatFailedMethodService(ApiRepository apiRepository, DbRepository dbRepository)
        {
            this.apiRepository = apiRepository;
            this.dbRepository = dbRepository;
        }

        public void StopTryMethodsCall()
        {
            alive = false;
        }

        public void StartTryMethodsCall()
        {
            if (!Settings.RepeatFailedMethod || alive)
                return;

            alive = true;
            Task.Run(
                () =>
                {
                    while (alive)
                    {
                        Thread.Sleep(Settings.RepeatFailMethodTimeoutMs);
                        RepeatAllFailedMethods();
                    }
                });
        }

        public void RepeatAllFailedMethods()
        {
            lock (syncObj)
            {
                if (isRepeating)
                    return;

                isRepeating = true;
            }

            var methods = dbRepository.GetFailedMethods();
            var type = apiRepository.GetType();
            foreach (var (id, MethodName, Parameters) in methods)
            {
                var method = type.GetMethod(MethodName);
                //try
                {
                    var res = method.Invoke(apiRepository,
                        Parameters
                        .Select(
                            x =>
                            {
                                var t = Type.GetType(x.TypeName);
                                if (t.IsEnum)
                                {
                                    return Enum.ToObject(t, x.Value);
                                }
                                else
                                    return Convert.ChangeType(x.Value, t);
                            })
                        .ToArray());
                    if (res is Task<bool> taskRes)
                    {
                        bool methodResult;
                        try
                        {
                            taskRes.Wait();
                            methodResult = taskRes.Result;
                        }
                        catch
                        {
                            break;
                        }

                        if (methodResult)
                        {
                            dbRepository.DeleteFailedMethod(id);
                        }
                        else
                            break;
                    }
                }
                /*catch
                {

                }*/
            }
        }
    }
}
