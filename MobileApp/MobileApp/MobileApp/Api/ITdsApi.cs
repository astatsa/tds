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
        [Post("/api/auth")]
        Task<AuthResult> Auth([Body]object authModel);//, CancellationToken cancellationToken);
    }
}
