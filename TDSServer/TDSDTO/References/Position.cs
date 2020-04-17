using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Text;

namespace TDSDTO.References
{
    public class Position : BaseModel
    {
        [DisplayName("Наименование")]
        public string Name { get; set; }
        [DisplayName("Описание")]
        public string Description { get; set; }
    }
}
