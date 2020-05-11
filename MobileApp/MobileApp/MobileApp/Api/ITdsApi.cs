using MobileApp.Models;
using Refit;
using System;
using System.Collections.Generic;
using System.Text;
using System.Threading;
using System.Threading.Tasks;
using DTO = TDSDTO;

namespace MobileApp.Api
{
    [Headers("Content-type: application/json", "Accept: application/json")]
    interface ITdsApi
    {
        [Post("/auth")]
        Task<AuthResult> Auth([Body]DTO.AuthModel authModel);//, CancellationToken cancellationToken);

        [Get("/orders/employee/{employee.Id}")]
        Task<ApiResult<ICollection<DTO.Documents.Order>>> GetOrders(DTO.References.Employee employee);

        [Get("/orders/current")]
        Task<ApiResult<DTO.Documents.Order>> GetCurrentOrder();

        [Post("/orders/{orderId}")]
        Task<ApiResult<bool>> SetOrderState(int orderId, [Body]DTO.OrderWeightAndState state);

        [Get("/gasstations")]
        Task<ApiResult<ICollection<DTO.References.GasStation>>> GetGasStations();

        [Post("/refuels")]
        Task<ApiResult<bool>> AddRefuel([Body] DTO.Documents.Refuel refuel);
    }
}
