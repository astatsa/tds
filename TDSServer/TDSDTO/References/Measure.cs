using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Runtime.CompilerServices;
using System.Text;

namespace TDSDTO.References
{
    public class Measure : BaseModel
    {
        [DisplayName("Краткое наименование")]
        public string Name { get; set; }
        [DisplayName("Полное наименование")]
        public string FullName { get; set; }
    }
}
