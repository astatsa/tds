﻿using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace TDSServer.Models
{
    public class Material : BaseModel
    {
        public string Name { get; set; }
        public string Description { get; set; }
        public int MeasureId { get; set; }
        public Measure Measure { get; set; }
    }
}
