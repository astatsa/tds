using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Runtime.CompilerServices;
using System.Text;

namespace TDSDTO.References
{
    public class Measure : BaseModel
    {
        [DisplayFormat("Краткое наименование")]
        public string Name { get; set; }
        [DisplayFormat("Полное наименование")]
        public string FullName { get; set; }
    }
}
