using Microsoft.Extensions.Options;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using TDSServer.Helpers;
using TDSServer.Models;

namespace TDSServer.Services
{
    public interface IUserService
    {
        Task<(User User, string Token)> AuthenticateAsync(string username, string password);
    }
}
