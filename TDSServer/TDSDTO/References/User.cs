using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Text;

namespace TDSDTO.References
{
    public class User : BaseModel
    {
        [DisplayName("Пользователь")]
        public string Username { get; set; }
        [DisplayName("Полное наименование")]
        public string FullName { get; set; }
    }
}
