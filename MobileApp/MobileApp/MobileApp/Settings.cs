﻿using System;
using System.Collections.Generic;
using System.Text;

namespace MobileApp
{
    static class Settings
    {
        public static string ServerUrl { get; set; } = "http://192.168.1.2:8099";
        public static int TimeoutMs { get; set; } = 10000;
    }
}