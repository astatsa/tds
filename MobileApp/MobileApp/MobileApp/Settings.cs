using System;
using System.Collections.Generic;
using System.Text;

namespace MobileApp
{
    static class Settings
    {
        public static string ServerUrl { get; set; } = "http://82.146.35.207:8888/api";//"http://10.0.2.2:5020/api";
        public static int TimeoutMs { get; set; } = 10000;
        public static int RepeatFailMethodTimeoutMs { get; set; } = 5000;
        public static bool RepeatFailedMethod { get; set; } = false;
    }
}
