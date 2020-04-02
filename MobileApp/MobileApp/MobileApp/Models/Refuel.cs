using System;
using System.Collections.Generic;
using System.Text;

namespace MobileApp.Models
{
    class Refuel : BaseModel
    {
        public GasStation GasStation { get; set; }
        public DateTime Date { get; set; }
        public Employee Driver { get; set; }
        public double Volume { get; set; }
    }
}
