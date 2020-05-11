using System;
using System.Collections.Generic;
using System.Text;
using DTO = TDSDTO.References;

namespace MobileApp.Models
{
    class AuthResult
    {
        public string Token { get; set; }
        public DTO.Employee Employee { get; set; }
    }
}
