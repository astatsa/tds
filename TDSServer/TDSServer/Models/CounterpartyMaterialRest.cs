﻿using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations.Schema;
using System.Linq;
using System.Threading.Tasks;

namespace TDSServer.Models
{
    public class CounterpartyMaterialRest
    {
        public int CounterpartyId { get; set; }
        [ForeignKey("CounterpartyId")]
        public Counterparty Counterparty { get; set; }
        public int MaterialId { get; set; }
        [ForeignKey("MaterialId")]
        public Material Material { get; set; }
        public double Rest { get; set; }
    }
}
