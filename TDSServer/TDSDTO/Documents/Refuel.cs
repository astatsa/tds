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
        [DisplayFormat("Дата", "dd.MM.yyyy HH:mm")]
        public DateTime Date { get; set; }
        public int DriverId { get; set; }
        [DisplayFormat("Водитель")]
        public string DriverName { get; set; }
        [DisplayFormat("Объем", horizontalAlignment: Alignments.Right)]
        public double Volume { get; set; }
    }
}
