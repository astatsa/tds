using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations.Schema;
using System.Linq;
using System.Threading.Tasks;

namespace TDSServer.Models
{
    public class Refuel : BaseModel
    {
        public GasStation GasStation { get; set; }
        public DateTime Date { get; set; }
        [ForeignKey("DriverId")]
        public Employee Driver { get; set; }
        public double Volume { get; set; }
    }
}
