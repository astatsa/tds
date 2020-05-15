using System;
using System.Collections.Generic;
using System.Text;

namespace TDSDTO.References
{
    public class Role : BaseModel
    {
        public string Name { get; set; }
        [DisplayFormat("Роль")]
        public string FullName { get; set; }
    }
}
