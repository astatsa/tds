using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Text;

namespace TDSDTO.References
{
    public class Material : BaseModel
    {
        [DisplayName("Наименование")]
        public string Name { get; set; }
        [DisplayName("Ед. изм.")]
        public string MeasureName { get; set; }
        public int MeasureId { get; set; }
        public string Description { get; set; }
    }
}
