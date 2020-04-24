using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Text;

namespace TDSDTO.References
{
    public class Material : BaseModel
    {
        [DisplayFormat("Наименование")]
        public string Name { get; set; }
        [DisplayFormat("Ед. изм.")]
        public string MeasureName { get; set; }
        public int MeasureId { get; set; }
        public string Description { get; set; }
    }
}
