using MobileApp.Models;
using Refit;
using System;
using System.Collections.Generic;
using System.Text;
using System.Threading;
using System.Threading.Tasks;

namespace MobileApp.Api
{
    [Headers("Content-type: application/json", "Accept: application/json")]
    interface ITdsApi
    {
        [Post("/auth")]
        Task<AuthResult> Auth([Body]object authModel);//, CancellationToken cancellationToken);

        [Get("/orders/employee/{employee.Id}")]
        Task<ApiResult<ICollection<Order>>> GetOrders(Employee employee);

        [Get("/orders/current")]
        Task<ApiResult<Order>> GetCurrentOrder();
    }
}
