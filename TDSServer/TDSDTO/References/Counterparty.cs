using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Text;

namespace TDSDTO.References
{
    public class Counterparty : BaseModel
    {
        [DisplayName("Наименование")]
        public string Name { get; set; }
        [DisplayName("Адрес")]
        public string Address { get; set; }
        [DisplayName("Описание")]
        public string Description { get; set; }
        [DisplayName("Тип")]
        public string TypeName { get; set; }
        public bool IsSupplier { get; set; }
    }
}
