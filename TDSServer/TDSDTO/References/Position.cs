using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Text;

namespace TDSDTO.References
{
    public class Position : BaseModel
    {
        [DisplayFormat("Наименование")]
        public string Name { get; set; }
        [DisplayFormat("Описание")]
        public string Description { get; set; }
    }
}
