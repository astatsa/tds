using MobileApp.Api;
using MobileApp.Models;
using System;
using System.Collections.Generic;
using System.Text;

namespace MobileApp
{
    static class SessionContext
    {
        public static string Token { get; set; }
        public static Employee Employee { get; set; }
        public static ITdsApi Api { get; set; }
    }
}
