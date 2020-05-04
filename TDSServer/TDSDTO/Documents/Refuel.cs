using System;
using System.Collections.Generic;
using System.Text;

namespace TDSDTO.Documents
{
    public class Refuel : BaseModel
    {
        public int GasStationId { get; set; }
        [DisplayFormat("АЗС")]
        public string GasStationName { get; set; }
        [DisplayFormat("Дата", "dd.MM.yyyy")]
        public DateTime Date { get; set; }
        public int DriverId { get; set; }
        [DisplayFormat("Водитель")]
        public string DriverName { get; set; }
        [DisplayFormat("Объем")]
        public double Volume { get; set; }
    }
}
