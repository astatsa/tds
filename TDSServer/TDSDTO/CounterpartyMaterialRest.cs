using System;
using System.Collections.Generic;
using System.IO;
using System.Text;

namespace TDSDTO
{
    public class CounterpartyMaterialRest
    {
        public int CounterpartyId { get; set; }
        public int MaterialId { get; set; }
        public string MaterialName { get; set; }
        public double Rest { get; set; }
    }
}
