using System;
using System.Collections.Generic;
using System.Net.Http;
using System.Text;
using System.Threading;
using System.Threading.Tasks;

namespace MobileApp.Api
{
    class ApiMessageHandler : DelegatingHandler
    {
        private Func<string> getToken;
        public ApiMessageHandler(HttpMessageHandler innerHandler, Func<string> funcGetToken)
        {
            getToken = funcGetToken;
            InnerHandler = innerHandler;
        }

        protected override Task<HttpResponseMessage> SendAsync(HttpRequestMessage request, CancellationToken cancellationToken)
        {
            var token = getToken?.Invoke();
            if (!String.IsNullOrWhiteSpace(token))
            {
                request.Headers.Authorization = new System.Net.Http.Headers.AuthenticationHeaderValue("Bearer", token);
            }
            return base.SendAsync(request, cancellationToken);
        }
    }
}
