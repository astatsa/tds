using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace TDSServer.Models
{
    public abstract class RegisterBaseModel
    {
        public int RegistratorTypeId { get; set; }
        public int RegistratorId { get; set; }
        public DateTime Date { get; set; }
    }
}
