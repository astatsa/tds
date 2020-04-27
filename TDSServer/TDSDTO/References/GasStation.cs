using System;
using System.Collections.Generic;
using System.Text;

namespace TDSDTO.References
{
    public class GasStation : BaseModel
    {
        [DisplayFormat("Наименование")]
        public string Name { get; set; }
        public string FullName { get; set; }
        [DisplayFormat("Адрес")]
        public string Address { get; set; }
        [DisplayFormat("Описание")]
        public string Description { get; set; }
    }
}
