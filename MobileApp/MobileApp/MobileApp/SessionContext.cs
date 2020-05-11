using MobileApp.Api;
using System;
using System.Collections.Generic;
using System.Text;
using TDSDTO.References;

namespace MobileApp
{
    static class SessionContext
    {
        public static string Token { get; set; }
        public static Employee Employee { get; set; }
    }
}
