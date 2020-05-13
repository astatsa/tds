using MobileApp.Api;
using Refit;
using System;
using System.Collections.Generic;
using System.Text;
using System.Threading.Tasks;
using TDSDTO.Documents;

namespace MobileApp.Repositories
{
    class ApiRepository
    {
        private readonly ITdsApi api;
        private readonly DbRepository dbRepository;

        public ApiRepository(ITdsApi api, DbRepository dbRepository)
        {
            this.api = api;
            this.dbRepository = dbRepository;
        }

        public async Task<bool> SetOrderState(int orderId, OrderStates state, double weight = 0, bool saveFailed = false)
        {
            try
            {
                var res = await api.SetOrderState(orderId,
                    new TDSDTO.OrderWeightAndState
                    {
                        OrderState = state,
                        Weight = weight
                    });
                if (!String.IsNullOrEmpty(res.Error))
                {
                    throw new Exception(res.Error);
                }
                if (res.Result)
                    return true;
            }
            catch (Exception)
            {

            }

            if (saveFailed)
            {
                dbRepository.SaveFailedMethodCall(nameof(SetOrderState), new object[] { orderId, state, weight, false });
            }

            return false;
        }
    }
}
