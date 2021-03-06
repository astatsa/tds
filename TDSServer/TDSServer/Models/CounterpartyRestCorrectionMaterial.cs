﻿using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;
using System.Linq;
using System.Threading.Tasks;

namespace TDSServer.Models
{
    public class CounterpartyRestCorrectionMaterial
    {
        public int Id { get; set; }
        public int MaterialId { get; set; }
        [ForeignKey("MaterialId")]
        public Material Material { get; set; }
        public double Correction { get; set; }
        public int CounterpartyRestCorrectionId { get; set; }
    }
}
